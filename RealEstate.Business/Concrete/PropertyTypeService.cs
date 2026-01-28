using System;
using System.Linq.Expressions;
using AutoMapper;
using RealEstate.Business.Abstract;
using RealEstate.Business.DTOs.PropertyTypeDto;
using RealEstate.Business.DTOs.ResponseDto;
using RealEstate.Data.Abstract;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Concrete;

public class PropertyTypeService : IPropertyTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRepository<PropertyType> _propertyTypeRepository;

    public PropertyTypeService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _propertyTypeRepository = _unitOfWork.GetRepository<PropertyType>();
    }

    public async Task<ResponseDto<PropertyTypeListDto>> GetAsync(int id)
    {
        try
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(id);

            if (propertyType == null)
            {
                return ResponseDto<PropertyTypeListDto>.Fail("Emlak türü bulunamadı", 404);
            }

            if (propertyType.IsDeleted)
            {
                return ResponseDto<PropertyTypeListDto>.Fail("Emlak türü silinmiş", 410);
            }

            var result = _mapper.Map<PropertyTypeListDto>(propertyType);
            return ResponseDto<PropertyTypeListDto>.Success(result, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<PropertyTypeListDto>.Fail(ex.Message, 500);
        }
    }

    public async Task<ResponseDto<IEnumerable<PropertyTypeListDto>>> GetAllAsync()
    {
        try
        {
            var propertyTypes = await _propertyTypeRepository.GetAllAsync(
                predicate: x => !x.IsDeleted
            );

            var result = _mapper.Map<IEnumerable<PropertyTypeListDto>>(propertyTypes);
            return ResponseDto<IEnumerable<PropertyTypeListDto>>.Success(result, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<IEnumerable<PropertyTypeListDto>>.Fail(ex.Message, 500);
        }
    }

    public async Task<ResponseDto<int>> CountAsync()
    {
        try
        {
            var count = await _propertyTypeRepository.CountAsync(x => !x.IsDeleted);
            return ResponseDto<int>.Success(count, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<int>.Fail(ex.Message, 500);
        }
    }

    public async Task<ResponseDto<PropertyTypeListDto>> CreateAsync(PropertyTypeCreateDto propertyTypeCreateDto)
    {
        try
        {
            var entity = _mapper.Map<PropertyType>(propertyTypeCreateDto);
            entity.CreatedAt = DateTimeOffset.UtcNow;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            entity.IsDeleted = false;

            await _propertyTypeRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<PropertyTypeListDto>(entity);
            return ResponseDto<PropertyTypeListDto>.Success(result, 201);
        }
        catch (Exception ex)
        {
            return ResponseDto<PropertyTypeListDto>.Fail(ex.Message, 500);
        }
    }

    public async Task<ResponseDto<NoContent>> UpdateAsync(int id, PropertyTypeUpdateDto propertyTypeUpdateDto)
    {
        try
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(id);

            if (propertyType == null)
            {
                return ResponseDto<NoContent>.Fail("Emlak türü bulunamadı", 404);
            }

            if (propertyType.IsDeleted)
            {
                return ResponseDto<NoContent>.Fail("Emlak türü silinmiş", 410);
            }

            _mapper.Map(propertyTypeUpdateDto, propertyType);
            propertyType.UpdatedAt = DateTimeOffset.UtcNow;

            await _propertyTypeRepository.UpdateAsync(propertyType);
            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail(ex.Message, 500);
        }
    }

    public async Task<ResponseDto<NoContent>> SoftDeleteAsync(int id)
    {
        try
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(id);

            if (propertyType == null)
            {
                return ResponseDto<NoContent>.Fail("Emlak türü bulunamadı", 404);
            }

            propertyType.IsDeleted = true;
            propertyType.UpdatedAt = DateTimeOffset.UtcNow;

            await _propertyTypeRepository.UpdateAsync(propertyType);
            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail(ex.Message, 500);
        }
    }

    public async Task<ResponseDto<NoContent>> HardDeleteAsync(int id)
    {
        try
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(id);

            if (propertyType == null)
            {
                return ResponseDto<NoContent>.Fail("Emlak türü bulunamadı", 404);
            }

            await _propertyTypeRepository.DeleteAsync(propertyType);
            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail(ex.Message, 500);
        }
    }
}
