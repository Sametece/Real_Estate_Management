using System;
using RealEstate.Entity.Enum;

namespace RealEstate.Business.DTOs;

public class EPropertyCreateDto
{
    //Bu dto agent ve admin için Yeni emlak yaratılırken kullanıcaklar
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? District { get; set; }

    // Fiziksel özellikler (create aşamasında girilir)
    public int Rooms { get; set; }
    public int Bathrooms { get; set; }
    public int Area { get; set; } // m²
    public int BuildYear { get; set; }
    public int Floor { get; set; }
    public int? TotalFloors { get; set; }



    public int PropertyTypeId { get; set; }
    public PropertyStatus Status { get; set; }
}
