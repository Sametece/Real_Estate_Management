using System;
using RealEstate.Entity.Enum;

namespace RealEstate.Business.DTOs;

public class PropertyFilterDto
{
    // Pagination
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    // Fiyat Filtreleri
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    // Konum Filtreleri
    public string? City { get; set; }
    public string? District { get; set; }

    // Fiziksel Özellik Filtreleri
    public int? MinRooms { get; set; }
    public int? MaxRooms { get; set; }
    public decimal? MinArea { get; set; }
    public decimal? MaxArea { get; set; }

    // Diğer Filtreler
    public int? PropertyTypeId { get; set; }
    public PropertyStatus? Status { get; set; }
    public int? AgentId { get; set; }
    public int? MinYear { get; set; }
    public int? MaxYear { get; set; }

    // Sıralama
    public string SortBy { get; set; } = "createdAt"; // price, area, rooms, createdAt
    public string SortOrder { get; set; } = "desc"; // asc, desc

    // Arama
    public string? SearchTerm { get; set; }
}