using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Business.DTOs;

public class ResetPasswordDto
{
    [Required(ErrorMessage = "Email adresi gereklidir")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Sıfırlama kodu gereklidir")]
    public string ResetToken { get; set; } = null!;

    [Required(ErrorMessage = "Yeni şifre gereklidir")]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
    public string NewPassword { get; set; } = null!;

    [Required(ErrorMessage = "Şifre tekrarı gereklidir")]
    [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor")]
    public string ConfirmNewPassword { get; set; } = null!;
}