using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Business.Abstract;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.ResponseDto;

namespace RealEstate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz model verisi", 400));
            }

            var result = await _authService.RegisterAsync(registerDto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz model verisi", 400));
            }

            var result = await _authService.LoginAsync(loginDto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        try
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(ResponseDto<object>.Fail("Refresh token gereklidir", 400));
            }

            var result = await _authService.RefreshTokenAsync(refreshToken);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz kullanıcı", 400));
            }

            var result = await _authService.LogoutAsync(userId);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz model verisi", 400));
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz kullanıcı", 400));
            }

            changePasswordDto.UserId = userId;
            var result = await _authService.ChangePasswordAsync(changePasswordDto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] string email)
    {
        try
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(ResponseDto<object>.Fail("Email adresi gereklidir", 400));
            }

            var result = await _authService.ForgotPasswordAsync(email);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDto<object>.Fail("Geçersiz model verisi", 400));
            }

            var result = await _authService.ResetPasswordAsync(resetPasswordDto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ResponseDto<object>.Fail($"Sunucu hatası: {ex.Message}", 500));
        }
    }
}