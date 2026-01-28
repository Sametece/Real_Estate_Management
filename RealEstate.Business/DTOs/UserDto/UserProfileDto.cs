using System;

namespace RealEstate.Business.DTOs;

public class UserProfileDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? ProfilePicture { get; set; }
    public bool IsAgent { get; set; }
    public string? AgencyName { get; set; }
    public string? LicenseNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<string> Roles { get; set; } = [];
}