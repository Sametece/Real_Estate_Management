using System;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealEstate.Business.Abstract;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.ResponseDto;
using RealEstate.Business.Extensions;
using RealEstate.Data.Abstract;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Concrete;

public class EPropertyService : IEPropertyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRepository<EProperty> _epropertyRepository;
    private readonly IRepository<PropertyType> _propertyTypeRepository;

    public EPropertyService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _epropertyRepository = _unitOfWork.GetRepository<EProperty>();
        _propertyTypeRepository = _unitOfWork.GetRepository<PropertyType>();
    }

    public async Task<ResponseDto<EPropertyListDto>> GetAsync(int id)
    {
        try
        {
            var property = await _epropertyRepository.GetAsync(
                predicate: x => x.Id == id && !x.IsDeleted,
                includes: q => q.Include(x => x.PropertyType)
            );

            if (property == null)
            {
                return ResponseDto<EPropertyListDto>.Fail("Emlak ilanı bulunamadı", 404);
            }

            var result = _mapper.Map<EPropertyListDto>(property);
            return ResponseDto<EPropertyListDto>.Success(result, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<EPropertyListDto>.Fail($"Emlak ilanı getirilirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<IEnumerable<EPropertyListDto>>> GetAllAsync(
        Expression<Func<EProperty, bool>>? predicate,
        Func<IQueryable<EProperty>, IOrderedQueryable<EProperty>>? orderBy,
        bool? propertyType = null,
        int? propertyTypeId = null,
        bool? isdeleted = null)
    {
        try
        {
            // Base predicate - soft delete kontrolü
            Expression<Func<EProperty, bool>> basePredicate = x => !x.IsDeleted;
            
            // Ek filtreler varsa birleştir
            if (predicate != null)
            {
                basePredicate = basePredicate.And(predicate);
            }

            if (propertyTypeId.HasValue)
            {
                basePredicate = basePredicate.And(x => x.PropertyTypeId == propertyTypeId.Value);
            }

            if (isdeleted.HasValue)
            {
                if (isdeleted.Value)
                {
                    basePredicate = x => x.IsDeleted;
                }
            }

            var properties = await _epropertyRepository.GetAllAsync(
                predicate: basePredicate,
                orderBy: orderBy,
                showIsDeleted: isdeleted ?? false,
                includes: propertyType == true ? 
                    new Func<IQueryable<EProperty>, IQueryable<EProperty>>[] { q => q.Include(x => x.PropertyType) } :
                    Array.Empty<Func<IQueryable<EProperty>, IQueryable<EProperty>>>()
            );

            var result = _mapper.Map<IEnumerable<EPropertyListDto>>(properties);
            return ResponseDto<IEnumerable<EPropertyListDto>>.Success(result, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<IEnumerable<EPropertyListDto>>.Fail($"Emlak ilanları getirilirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<EPropertyPagedResultDto<EPropertyListDto>>> GetAllPagedAsync(
        EPropertyPaginationQueryDto ePropertyPaginationQueryDto,
        Expression<Func<EProperty, bool>>? predicate = null,
        Func<IQueryable<EProperty>, IOrderedQueryable<EProperty>>? orderBy = null,
        bool? propertyType = false,
        int? propertyTypeId = null,
        bool? isdeleted = null)
    {
        try
        {
            // Base predicate - soft delete kontrolü
            Expression<Func<EProperty, bool>> basePredicate = x => !x.IsDeleted;
            
            // Ek filtreler varsa birleştir
            if (predicate != null)
            {
                basePredicate = basePredicate.And(predicate);
            }

            if (propertyTypeId.HasValue)
            {
                basePredicate = basePredicate.And(x => x.PropertyTypeId == propertyTypeId.Value);
            }

            if (isdeleted.HasValue)
            {
                if (isdeleted.Value)
                {
                    basePredicate = x => x.IsDeleted;
                }
            }

            var (data, totalCount) = await _epropertyRepository.GetPagedAsync(
                predicate: basePredicate,
                orderBy: orderBy,
                skip: ePropertyPaginationQueryDto.Skip,
                take: ePropertyPaginationQueryDto.Take,
                showIsDeleted: isdeleted ?? false,
                includes: propertyType == true ? 
                    new Func<IQueryable<EProperty>, IQueryable<EProperty>>[] { q => q.Include(x => x.PropertyType) } :
                    Array.Empty<Func<IQueryable<EProperty>, IQueryable<EProperty>>>()
            );

            var mappedData = _mapper.Map<IEnumerable<EPropertyListDto>>(data);
            var pagedResult = EPropertyPagedResultDto<EPropertyListDto>.Create(
                mappedData,
                ePropertyPaginationQueryDto.PageNumber,
                ePropertyPaginationQueryDto.PageSize,
                totalCount
            );

            return ResponseDto<EPropertyPagedResultDto<EPropertyListDto>>.Success(pagedResult, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<EPropertyPagedResultDto<EPropertyListDto>>.Fail($"Sayfalanmış emlak ilanları getirilirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<int>> CountAsync()
    {
        try
        {
            var count = await _epropertyRepository.CountAsync(x => !x.IsDeleted);
            return ResponseDto<int>.Success(count, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<int>.Fail($"Emlak ilanı sayısı alınırken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<EPropertyListDto>> CreateAsync(EPropertyCreateDto ePropertyCreateDto)
    {
        try
        {
            // PropertyType kontrolü
            var propertyTypeExists = await _propertyTypeRepository.ExistsAsync(x => x.Id == ePropertyCreateDto.PropertyTypeId && !x.IsDeleted);
            if (!propertyTypeExists)
            {
                return ResponseDto<EPropertyListDto>.Fail("Geçersiz emlak türü", 400);
            }

            var entity = _mapper.Map<EProperty>(ePropertyCreateDto);
            entity.CreatedAt = DateTimeOffset.UtcNow;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            entity.IsDeleted = false;
            entity.YearBuilt = ePropertyCreateDto.BuildYear;

            await _epropertyRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            // PropertyType ile birlikte getir
            var createdProperty = await _epropertyRepository.GetAsync(
                predicate: x => x.Id == entity.Id,
                includes: q => q.Include(x => x.PropertyType)
            );

            var result = _mapper.Map<EPropertyListDto>(createdProperty);
            return ResponseDto<EPropertyListDto>.Success(result, 201);
        }
        catch (Exception ex)
        {
            return ResponseDto<EPropertyListDto>.Fail($"Emlak ilanı oluşturulurken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> UpdateAsync(int id, EPropertyAdminUpdateDto ePropertyAdminUpdateDto)
    {
        try
        {
            var property = await _epropertyRepository.GetAsync(x => x.Id == id && !x.IsDeleted);

            if (property == null)
            {
                return ResponseDto<NoContent>.Fail("Emlak ilanı bulunamadı", 404);
            }

            // PropertyType kontrolü
            if (ePropertyAdminUpdateDto.PropertyTypeId.HasValue)
            {
                var propertyTypeExists = await _propertyTypeRepository.ExistsAsync(x => x.Id == ePropertyAdminUpdateDto.PropertyTypeId.Value && !x.IsDeleted);
                if (!propertyTypeExists)
                {
                    return ResponseDto<NoContent>.Fail("Geçersiz emlak türü", 400);
                }
            }

            _mapper.Map(ePropertyAdminUpdateDto, property);
            property.UpdatedAt = DateTimeOffset.UtcNow;

            _epropertyRepository.Update(property);
            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Emlak ilanı güncellenirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> Update2Async(int id, EPropertyAgentUpdateDto ePropertyAgentUpdateDto)
    {
        try
        {
            var property = await _epropertyRepository.GetAsync(x => x.Id == id && !x.IsDeleted);

            if (property == null)
            {
                return ResponseDto<NoContent>.Fail("Emlak ilanı bulunamadı", 404);
            }

            _mapper.Map(ePropertyAgentUpdateDto, property);
            property.UpdatedAt = DateTimeOffset.UtcNow;

            _epropertyRepository.Update(property);
            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Emlak ilanı güncellenirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> SoftDeleteAsync(int id)
    {
        try
        {
            var property = await _epropertyRepository.GetAsync(x => x.Id == id && !x.IsDeleted);

            if (property == null)
            {
                return ResponseDto<NoContent>.Fail("Emlak ilanı bulunamadı", 404);
            }

            property.IsDeleted = true;
            property.UpdatedAt = DateTimeOffset.UtcNow;

            _epropertyRepository.Update(property);
            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Emlak ilanı silinirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> HardDeleteAsync(int id)
    {
        try
        {
            var property = await _epropertyRepository.GetAsync(x => x.Id == id);

            if (property == null)
            {
                return ResponseDto<NoContent>.Fail("Emlak ilanı bulunamadı", 404);
            }

            _epropertyRepository.Remove(property);
            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Emlak ilanı kalıcı olarak silinirken hata oluştu: {ex.Message}", 500);
        }
    }
}
