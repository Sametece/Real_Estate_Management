using System;

namespace RealEstate.Business.DTOs.PropertyTypeDto;

public class PropertyTypeUpdateDto
{
   public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }
}
