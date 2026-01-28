using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Business.DTOs;

public class UserUpdateDto
{
    [Required(ErrorMessage = "Ad gereklidir")]
    [MinLength(2, ErrorMessage = "Ad en az 2 karakter olmalıdır")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Soyad gereklidir")]
    [MinLength(2, ErrorMessage = "Soyad en az 2 karakter olmalıdır")]
    public string LastName { get; set; } = null!;

    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
    public string? PhoneNumber { get; set; }

    public string? ProfilePicture { get; set; }
}