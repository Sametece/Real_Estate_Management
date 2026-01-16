using System;

namespace RealEstate.Business.DTOs;

public class EPropertyPagedResultDto<T>
{
    public IEnumerable<T> Data { get; set; } = [];
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;

    public EPropertyPagedResultDto(IEnumerable<T> data, int pageNumber, int pageSize, int totalCount)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public static EPropertyPagedResultDto<T> Create(IEnumerable<T> data, int pageNumber, int pageSize, int totalCount)
    {
        return new EPropertyPagedResultDto<T>(data, pageNumber, pageSize, totalCount);
    }
}
//Sayfalama da kalında 