using System;
using Microsoft.AspNetCore.Identity;
using RealEstate.Entity.Abstract;

namespace RealEstate.Entity.Concrete;

public class AppUser : IdentityUser<int>
{
    // Kişisel Bilgiler
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? ProfilePicture { get; set; }

    // Emlakçı Bilgileri
    public bool IsAgent { get; set; } = false;
    public string? AgencyName { get; set; }
    public string? LicenseNumber { get; set; }

    // BaseClass özellikleri
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    // Navigation Properties
    public ICollection<EProperty> Properties { get; set; } = [];
    public ICollection<Inquiry> Inquiries { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}