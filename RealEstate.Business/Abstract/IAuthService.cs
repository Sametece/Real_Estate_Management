using System;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.ResponseDto;

namespace RealEstate.Business.Abstract;

public interface IAuthService
{
    /// <summary>
    /// Kullanıcı girişi
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns></returns>
    Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto loginDto);

    /// <summary>
    /// Kullanıcı kaydı
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    Task<ResponseDto<RegisterResponseDto>> RegisterAsync(RegisterDto registerDto);

    /// <summary>
    /// Token yenileme
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    Task<ResponseDto<LoginResponseDto>> RefreshTokenAsync(string refreshToken);

    /// <summary>
    /// Çıkış yapma
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> LogoutAsync(int userId);

    /// <summary>
    /// Şifre değiştirme
    /// </summary>
    /// <param name="changePasswordDto"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> ChangePasswordAsync(ChangePasswordDto changePasswordDto);

    /// <summary>
    /// Şifre sıfırlama isteği
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> ForgotPasswordAsync(string email);

    /// <summary>
    /// Şifre sıfırlama
    /// </summary>
    /// <param name="resetPasswordDto"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}