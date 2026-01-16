using System;
using RealEstate.Entity.Enum;

namespace RealEstate.Business.DTOs;

public class EPropertyAgentUpdateDto
{
   //Emlakçı ilanını güncellerken 

   public string Title { get; set; } = null!;
   public string  Description { get; set; } = null!;

   public decimal Price { get; set; }
   public PropertyStatus Status { get; set; }

   //İlan oluştuktan sonra emlakçı her özelliği güncelleyemesin!
}
