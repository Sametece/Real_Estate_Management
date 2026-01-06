using System;
using RealEstate.Entity.Abstract;
using RealEstate.Entity.Enum;

namespace RealEstate.Entity.Concrete;

public class Inquiry : BaseClass
{
   public string Name { get; set; } = string.Empty;

   public string Email { get; set; } = string.Empty;

   public string? Phone { get; set; }

   public string Message { get; set; } = string.Empty;

   // Durum
    public InquiryStatus Status { get; set; }

    // İlişkiler
    public int PropertyId { get; set; }
    public Property Property { get; set; } = null!;

    public int? UserId { get; set; }                       
    public AppUser? User { get; set; }

    
}
