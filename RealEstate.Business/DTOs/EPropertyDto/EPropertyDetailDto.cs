using System;
using RealEstate.Entity.Enum;

namespace RealEstate.Business.DTOs;

public class EPropertyDetailDto
{

    //ilana girdikten sonraki ekran
   public int Id { get; set; }

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string District { get; set; } = null!;

    public int Rooms { get; set; }
    public int Bathrooms { get; set; }
    public int Area { get; set; }
    public int BuildYear { get; set; }

    public PropertyStatus Status { get; set; }

    public string PropertyTypeName { get; set; } = null!;
    public string AgentName { get; set; } = null!;

    public List<PropertyImageDto> Images { get; set; } = new();
}

