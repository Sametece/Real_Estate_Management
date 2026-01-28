using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Business.Abstract;
using RealEstate.Business.Configuration;
using RealEstate.Business.DTOs.ResponseDto;
using RealEstate.Data.Abstract;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Concrete;

public class TokenService : ITokenService
{
    private readonly JwtConfig _jwtConfig;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<RefreshToken> _refreshTokenRepository;

    public TokenService(IOptions<JwtConfig> jwtConfig, IUnitOfWork unitOfWork)
    {
        _jwtConfig = jwtConfig.Value;
        _unitOfWork = unitOfWork;
        _refreshTokenRepository = _unitOfWork.GetRepository<RefreshToken>();
    }

    public async Task<string> GenerateAccessTokenAsync(AppUser user, IList<string> roles)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new("firstName", user.FirstName),
                new("lastName", user.LastName),
                new("isAgent", user.IsAgent.ToString().ToLower())
            };

            // Rolleri ekle
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpiration),
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Access token oluşturulurken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task<ResponseDto<RefreshToken>> GenerateRefreshTokenAsync(int userId)
    {
        try
        {
            var refreshToken = new RefreshToken
            {
                Token = GenerateRandomToken(),
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiration),
                UserId = userId,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            await _refreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.SaveAsync();

            return ResponseDto<RefreshToken>.Success(refreshToken, 201);
        }
        catch (Exception ex)
        {
            return ResponseDto<RefreshToken>.Fail($"Refresh token oluşturulurken hata oluştu: {ex.Message}", 500);
        }
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Secret)),
                ValidateLifetime = false, // Expired token'ları da kabul et
                ValidIssuer = _jwtConfig.Issuer,
                ValidAudience = _jwtConfig.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }

    public async Task<ResponseDto<RefreshToken>> ValidateRefreshTokenAsync(string token)
    {
        try
        {
            var refreshToken = await _refreshTokenRepository.GetAsync(
                predicate: x => x.Token == token && !x.IsDeleted
            );

            if (refreshToken == null)
            {
                return ResponseDto<RefreshToken>.Fail("Geçersiz refresh token", 401);
            }

            if (!refreshToken.IsActive)
            {
                return ResponseDto<RefreshToken>.Fail("Refresh token süresi dolmuş veya iptal edilmiş", 401);
            }

            return ResponseDto<RefreshToken>.Success(refreshToken, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<RefreshToken>.Fail($"Refresh token doğrulanırken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> RevokeRefreshTokenAsync(string token, string reason)
    {
        try
        {
            var refreshToken = await _refreshTokenRepository.GetAsync(
                predicate: x => x.Token == token && !x.IsDeleted
            );

            if (refreshToken == null)
            {
                return ResponseDto<NoContent>.Fail("Refresh token bulunamadı", 404);
            }

            refreshToken.IsRevoked = true;
            refreshToken.ReasonRevoked = reason;
            refreshToken.UpdatedAt = DateTimeOffset.UtcNow;

            _refreshTokenRepository.Update(refreshToken);
            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Refresh token iptal edilirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> RevokeAllRefreshTokensAsync(int userId, string reason)
    {
        try
        {
            var refreshTokens = await _refreshTokenRepository.GetAllAsync(
                predicate: x => x.UserId == userId && !x.IsRevoked && !x.IsDeleted,
                showIsDeleted: false,
                includes: Array.Empty<Func<IQueryable<RefreshToken>, IQueryable<RefreshToken>>>()
            );

            foreach (var token in refreshTokens)
            {
                token.IsRevoked = true;
                token.ReasonRevoked = reason;
                token.UpdatedAt = DateTimeOffset.UtcNow;
                _refreshTokenRepository.Update(token);
            }

            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Refresh token'lar iptal edilirken hata oluştu: {ex.Message}", 500);
        }
    }

    private static string GenerateRandomToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}