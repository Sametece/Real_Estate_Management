using System;
using RealEstate.Entity.Enum;

namespace RealEstate.Business.DTOs;

public class EPropertyAdminUpdateDto
{
    //Admin bir ilanı güncellerken tüm özelliklerine erişebilir
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }

    // Fiziksel özellikler
    public int? Rooms { get; set; }
    public int? Bathrooms { get; set; }
    public int? Area { get; set; }
    public int? BuildYear { get; set; }
    public int? Floor { get; set; }
    public int? TotalFloors { get; set; }

    public int? PropertyTypeId { get; set; }
    public PropertyStatus? Status { get; set; }
}
