using System;
using RealEstate.Entity.Enum;

namespace RealEstate.Business.DTOs;

public class EPropertyListDto
{ 
   //Listelemek için sayfada göstermek için aramak için kullanılıcak dto 
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    public decimal Price { get; set; }

    public string City { get; set; } = null!;
    public string District { get; set; } = null!;

    public int Rooms { get; set; }
    public int Area { get; set; }

    public PropertyStatus Status { get; set; }

    public string PropertyTypeName { get; set; } = null!;
   
   public string? CoverImage { get; set; }
}
