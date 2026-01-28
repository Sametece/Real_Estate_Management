using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Business.DTOs;

public class ChangePasswordDto
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Mevcut şifre gereklidir")]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = "Yeni şifre gereklidir")]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
    public string NewPassword { get; set; } = null!;

    [Required(ErrorMessage = "Şifre tekrarı gereklidir")]
    [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor")]
    public string ConfirmNewPassword { get; set; } = null!;
}