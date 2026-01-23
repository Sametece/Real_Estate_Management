using System;
using System.Linq.Expressions;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.ResponseDto;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Abstract;

public interface IEPropertyService
{
    
   Task<ResponseDto<EPropertyListDto>> GetAsync(int id);

   Task<ResponseDto<IEnumerable<EPropertyListDto>>> GetAllAsync(
    Expression<Func<EProperty, bool>>? predicate,
    Func<IQueryable<EProperty>, IOrderedQueryable<EProperty>>? orderBy,
    bool? propertyType = null,
    int? propertyTypeId = null,
    bool? isdeleted = null);

    Task<ResponseDto<EPropertyPagedResultDto<EPropertyListDto>>> GetAllPagedAsync(
        EPropertyPaginationQueryDto ePropertyPaginationQueryDto ,
        Expression<Func<EProperty, bool>>? predicate = null,
        Func<IQueryable<EProperty>, IOrderedQueryable<EProperty>>? orderBy = null,
        bool? propertyType = false,
    int? propertyTypeId = null,
    bool? isdeleted = null);


   Task<ResponseDto<int>> CountAsync();

Task<ResponseDto<EPropertyListDto>> CreateAsync(EPropertyCreateDto ePropertyCreateDto);

Task<ResponseDto<NoContent>> UpdateAsync(int id, EPropertyAdminUpdateDto ePropertyAdminUpdateDto);
Task<ResponseDto<NoContent>> Update2Async(int id, EPropertyAgentUpdateDto ePropertyAgentUpdateDto);

Task<ResponseDto<NoContent>>  SoftDeleteAsync(int id);

Task<ResponseDto<NoContent>> HardDeleteAsync(int id);


}
