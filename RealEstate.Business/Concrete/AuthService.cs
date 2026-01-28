using System;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RealEstate.Business.Abstract;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.ResponseDto;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Concrete;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || user.IsDeleted)
            {
                return ResponseDto<LoginResponseDto>.Fail("Geçersiz email veya şifre", 401);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return ResponseDto<LoginResponseDto>.Fail("Geçersiz email veya şifre", 401);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = await _tokenService.GenerateAccessTokenAsync(user, roles);
            var refreshTokenResult = await _tokenService.GenerateRefreshTokenAsync(user.Id);

            if (!refreshTokenResult.IsSucceed)
            {
                return ResponseDto<LoginResponseDto>.Fail("Token oluşturulurken hata oluştu", 500);
            }

            var response = new LoginResponseDto
            {
                UserId = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = roles.FirstOrDefault() ?? "User",
                AccessToken = accessToken,
                RefreshToken = refreshTokenResult.Data!.Token,
                TokenExpiry = DateTime.UtcNow.AddMinutes(30)
            };

            return ResponseDto<LoginResponseDto>.Success(response, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<LoginResponseDto>.Fail($"Giriş yapılırken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<RegisterResponseDto>> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return ResponseDto<RegisterResponseDto>.Fail("Bu email adresi zaten kullanılıyor", 400);
            }

            var user = new AppUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                IsAgent = registerDto.Role.Equals("Agent", StringComparison.OrdinalIgnoreCase),
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseDto<RegisterResponseDto>.Fail($"Kullanıcı oluşturulamadı: {errors}", 400);
            }

            // Rol atama
            var roleResult = await _userManager.AddToRoleAsync(user, registerDto.Role);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user); // Rollback
                return ResponseDto<RegisterResponseDto>.Fail("Rol ataması başarısız", 400);
            }

            var response = new RegisterResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = registerDto.Role,
                CreatedAt = user.CreatedAt.DateTime
            };

            return ResponseDto<RegisterResponseDto>.Success(response, 201);
        }
        catch (Exception ex)
        {
            return ResponseDto<RegisterResponseDto>.Fail($"Kayıt olurken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<LoginResponseDto>> RefreshTokenAsync(string refreshToken)
    {
        try
        {
            var tokenResult = await _tokenService.ValidateRefreshTokenAsync(refreshToken);
            if (!tokenResult.IsSucceed)
            {
                return ResponseDto<LoginResponseDto>.Fail("Geçersiz refresh token", 401);
            }

            var user = await _userManager.FindByIdAsync(tokenResult.Data!.UserId.ToString());
            if (user == null || user.IsDeleted)
            {
                return ResponseDto<LoginResponseDto>.Fail("Kullanıcı bulunamadı", 404);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = await _tokenService.GenerateAccessTokenAsync(user, roles);
            var newRefreshTokenResult = await _tokenService.GenerateRefreshTokenAsync(user.Id);

            if (!newRefreshTokenResult.IsSucceed)
            {
                return ResponseDto<LoginResponseDto>.Fail("Yeni token oluşturulamadı", 500);
            }

            // Eski token'ı revoke et
            await _tokenService.RevokeRefreshTokenAsync(refreshToken, "Replaced by new token");

            var response = new LoginResponseDto
            {
                UserId = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = roles.FirstOrDefault() ?? "User",
                AccessToken = accessToken,
                RefreshToken = newRefreshTokenResult.Data!.Token,
                TokenExpiry = DateTime.UtcNow.AddMinutes(30)
            };

            return ResponseDto<LoginResponseDto>.Success(response, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<LoginResponseDto>.Fail($"Token yenilenirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> LogoutAsync(int userId)
    {
        try
        {
            await _tokenService.RevokeAllRefreshTokensAsync(userId, "User logout");
            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Çıkış yapılırken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(changePasswordDto.UserId.ToString());
            if (user == null || user.IsDeleted)
            {
                return ResponseDto<NoContent>.Fail("Kullanıcı bulunamadı", 404);
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseDto<NoContent>.Fail($"Şifre değiştirilemedi: {errors}", 400);
            }

            // Tüm refresh token'ları revoke et (güvenlik için)
            await _tokenService.RevokeAllRefreshTokensAsync(user.Id, "Password changed");

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Şifre değiştirilirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> ForgotPasswordAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.IsDeleted)
            {
                // Güvenlik için her zaman başarılı döndür
                return ResponseDto<NoContent>.Success(204);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            // TODO: Email gönderme servisi implementasyonu
            // await _emailService.SendPasswordResetEmailAsync(user.Email, token);

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Şifre sıfırlama isteği gönderilirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null || user.IsDeleted)
            {
                return ResponseDto<NoContent>.Fail("Geçersiz istek", 400);
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.ResetToken, resetPasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseDto<NoContent>.Fail($"Şifre sıfırlanamadı: {errors}", 400);
            }

            // Tüm refresh token'ları revoke et
            await _tokenService.RevokeAllRefreshTokensAsync(user.Id, "Password reset");

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Şifre sıfırlanırken hata oluştu: {ex.Message}", 500);
        }
    }
}