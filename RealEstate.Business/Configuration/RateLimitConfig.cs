using System;

namespace RealEstate.Business.Configuration;

public class RateLimitConfig
{
    public int GeneralRequests { get; set; } = 100; // per minute
    public int AuthRequests { get; set; } = 5; // per minute
    public int SearchRequests { get; set; } = 30; // per minute
    public int CreateRequests { get; set; } = 10; // per minute
}