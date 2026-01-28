using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Business.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Email adresi gereklidir")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Şifre gereklidir")]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; } = false;
}