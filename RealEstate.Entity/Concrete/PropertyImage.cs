using System;
using RealEstate.Entity.Abstract;

namespace RealEstate.Entity.Concrete;

public class PropertyImage : BaseClass
{
   public string ImageUrl { get; set; } = string.Empty;

   public int DisplayOrder { get; set; }

   public bool IsPrimary { get; set; } 
   
   //Navi prop
   public int PropertyId { get; set; } 
   public Property Property { get; set; } = null! ;

}
