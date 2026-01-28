using System;

namespace RealEstate.Business.DTOs;

public class PropertyImageUpdateDto
{
    public string? ImageUrl { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsPrimary { get; set; }
}