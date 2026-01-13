using System;
using RealEstate.Entity.Abstract;
using RealEstate.Entity.Enum;

namespace RealEstate.Entity.Concrete;

public class EProperty:BaseClass
{
    //Temel özellikler
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }

    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? District { get; set; }


    //Fiziksel özellikler

    public int Rooms { get; set; }
    public int? Bathrooms { get; set; }
    public decimal Area { get; set; }
    public int Floor { get; set; }
    public int? TotalFloors { get; set; }
    public int YearBuilt { get; set; }

    public PropertyStatus Status { get; set; }

    // FK
    public int PropertyTypeId { get; set; }
    public string AgentId { get; set; } = null!;

    // Navi proplar
    public PropertyType? PropertyType { get; set; } = null!;
   // public AppUser Agent { get; set; } = null!;
    public ICollection<PropertyImage>? Images { get; set; } =  [];
    public ICollection<Inquiry>? Inquiries { get; set; } = [];
}

