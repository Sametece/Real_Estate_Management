using System;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.PropertyTypeDto;
using RealEstate.Business.DTOs.ResponseDto;

namespace RealEstate.Business.Abstract;

public interface IPropertyTypeService
{
    Task<ResponseDto<PropertyTypeListDto>> GetAsync(int id);
    
    Task<ResponseDto<IEnumerable<PropertyTypeListDto>>> GetAllAsync();

    Task<ResponseDto<int>> CountAsync();

    Task<ResponseDto<PropertyTypeListDto>> CreateAsync(PropertyTypeCreateDto propertyTypeCreateDto);

    Task<ResponseDto<NoContent>> UpdateAsync(int id, PropertyTypeUpdateDto propertyTypeUpdateDto);

    Task<ResponseDto<NoContent>> SoftDeleteAsync(int id);

    Task<ResponseDto<NoContent>> HardDeleteAsync(int id);
}
