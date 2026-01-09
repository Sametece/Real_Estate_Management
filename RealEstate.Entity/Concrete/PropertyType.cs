using System;
using RealEstate.Entity.Abstract;

namespace RealEstate.Entity.Concrete;

public class PropertyType:BaseClass
{
   public string Name { get; set; } = null!;
    public string? Description { get; set; }

    // Navigation
    public ICollection<EProperty>  EProperties { get; set; } = []; 
}
