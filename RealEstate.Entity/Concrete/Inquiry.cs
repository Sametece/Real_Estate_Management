using System;
using RealEstate.Entity.Abstract;
using RealEstate.Entity.Enum;

namespace RealEstate.Entity.Concrete;

public class Inquiry:BaseClass
{
  public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string Message { get; set; } = null!;

    public InquiryStatus Status { get; set; } 

    public int PropertyId { get; set; }
    public EProperty Property { get; set; } = null!;

    public int? UserId { get; set; }                       
    public AppUser? User { get; set; }
}
