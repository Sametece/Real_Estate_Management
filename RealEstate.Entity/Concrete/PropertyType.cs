using System;
using RealEstate.Entity.Abstract;
using RealEstate.Entity.Concrete;

namespace RealEstate.Entity.Enum;

public class PropertyType : BaseClass
{
    public string Name { get; set; } = string.Empty;

    public string? Description {get; set;} 

    //navi

    public ICollection<Property> Properties {get; set;} = [];
}
