using System;

namespace RealEstate.Business.Configuration;

public class JwtConfig
{
    public string Secret { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int AccessTokenExpiration { get; set; } = 30; // minutes
    public int RefreshTokenExpiration { get; set; } = 7; // days
}