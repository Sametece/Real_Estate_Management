using System;

namespace RealEstate.Business.DTOs;

public class PropertyImageCreateDto
{
    public string ImageUrl { get; set; } = null!;
    public int DisplayOrder { get; set; } = 0;
    public bool IsPrimary { get; set; } = false;
    public int PropertyId { get; set; }
}