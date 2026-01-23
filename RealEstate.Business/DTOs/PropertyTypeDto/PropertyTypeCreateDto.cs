using System;

namespace RealEstate.Business.DTOs.PropertyTypeDto;

public class PropertyTypeCreateDto
{
  // Sadece Admin oluşturur

  public string Name { get; set; } = null!;
  public string Description { get; set; } = null!;

}
