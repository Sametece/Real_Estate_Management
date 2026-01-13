using System;
using RealEstate.Entity.Abstract;

namespace RealEstate.Entity.Concrete;

public class PropertyImage:BaseClass
{
   public string ImageUrl { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }

   public int PropertyId { get; set; } 
   public EProperty? Property { get; set; } = null! ;
}
