using System;
using RealEstate.Entity.Enum;

namespace RealEstate.Business.DTOs.EPropertyDto;

public class EPropertyFilterDto
{
    //Filtreleme işlemleri için dto
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    public string? City { get; set; }
    public string? District { get; set; }

    public int? MinRooms { get; set; }
    public int? MaxRooms { get; set; }

    public int? MinArea { get; set; }
    public int? MaxArea { get; set; }

    public int? PropertyTypeId { get; set; }
    public PropertyStatus? Status { get; set; }

    public int? AgentId { get; set; }

    public int? MinYear { get; set; }
    public int? MaxYear { get; set; }
}
