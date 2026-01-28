using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Business.DTOs;

public class AgentUpdateDto
{
    [Required(ErrorMessage = "Şirket adı gereklidir")]
    [MinLength(2, ErrorMessage = "Şirket adı en az 2 karakter olmalıdır")]
    public string AgencyName { get; set; } = null!;

    [Required(ErrorMessage = "Lisans numarası gereklidir")]
    public string LicenseNumber { get; set; } = null!;
}