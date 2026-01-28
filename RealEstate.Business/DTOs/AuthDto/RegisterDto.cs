using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Business.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Ad gereklidir")]
    [MinLength(2, ErrorMessage = "Ad en az 2 karakter olmalıdır")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Soyad gereklidir")]
    [MinLength(2, ErrorMessage = "Soyad en az 2 karakter olmalıdır")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Email adresi gereklidir")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Şifre gereklidir")]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Şifre tekrarı gereklidir")]
    [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
    public string ConfirmPassword { get; set; } = null!;

    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
    public string? PhoneNumber { get; set; }

    public string Role { get; set; } = "User"; // Default role
}