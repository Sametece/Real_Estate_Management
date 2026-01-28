using System;
using System.Security.Claims;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.ResponseDto;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Abstract;

public interface ITokenService
{
    /// <summary>
    /// Access token oluştur
    /// </summary>
    /// <param name="user"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    Task<string> GenerateAccessTokenAsync(AppUser user, IList<string> roles);

    /// <summary>
    /// Refresh token oluştur
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ResponseDto<RefreshToken>> GenerateRefreshTokenAsync(int userId);

    /// <summary>
    /// Token'dan claims'leri al
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

    /// <summary>
    /// Refresh token'ı validate et
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<ResponseDto<RefreshToken>> ValidateRefreshTokenAsync(string token);

    /// <summary>
    /// Refresh token'ı revoke et
    /// </summary>
    /// <param name="token"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> RevokeRefreshTokenAsync(string token, string reason);

    /// <summary>
    /// Kullanıcının tüm refresh token'larını revoke et
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> RevokeAllRefreshTokensAsync(int userId, string reason);
}