using System;
using System.ComponentModel.DataAnnotations;
using RealEstate.Entity.Abstract;
using RealEstate.Entity.Enum;

namespace RealEstate.Entity.Concrete;

public class Property : BaseClass
{ 
    // Temel Bilgiler
    public string Title { get; set; } = string.Empty;           // zorunlu
    public string Description { get; set; } = string.Empty;     // zorunlu
    public decimal Price { get; set; }                           // zorunlu
    public string Address { get; set; } = string.Empty;          // zorunlu
    public string City { get; set; } = string.Empty;             // zorunlu
    public string? District { get; set; }                        // opsiyonel

    // Fiziksel Özellikler
    public int Rooms { get; set; }                               // zorunlu
    public int? Bathrooms { get; set; }                          // opsiyonel
    public decimal Area { get; set; }                            // zorunlu
    public int Floor { get; set; }                               // zorunlu
    public int? TotalFloors { get; set; }                        // opsiyonel
    public int YearBuilt { get; set; }                           // zorunlu

    // Durum
    public PropertyStatus Status { get; set; }

    // İlişkiler
    public int PropertyTypeId { get; set; }
    public PropertyType PropertyType { get; set; } = null!;

    public int AgentId { get; set; }
    public AppUser Agent { get; set; } = null!;
    
    public ICollection<PropertyImage> Images { get; set; } = [];
    public ICollection<Inquiry> Inquiries { get; set; } = [];

} 
