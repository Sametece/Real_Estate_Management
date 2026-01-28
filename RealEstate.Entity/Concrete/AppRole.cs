using System;
using Microsoft.AspNetCore.Identity;

namespace RealEstate.Entity.Concrete;

public class AppRole : IdentityRole<int>
{
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}