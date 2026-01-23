using System;

namespace RealEstate.Business.DTOs;

public class PropertyImageDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = null!;
    public bool IsPrimary { get; set; }
}
