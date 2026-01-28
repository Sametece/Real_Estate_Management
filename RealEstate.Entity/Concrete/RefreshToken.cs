using System;
using RealEstate.Entity.Abstract;

namespace RealEstate.Entity.Concrete;

public class RefreshToken : BaseClass
{
    public string Token { get; set; } = null!;
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; } = false;
    public string? ReplacedByToken { get; set; }
    public string? ReasonRevoked { get; set; }

    // Foreign Key
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
    public bool IsActive => !IsRevoked && !IsExpired;
}