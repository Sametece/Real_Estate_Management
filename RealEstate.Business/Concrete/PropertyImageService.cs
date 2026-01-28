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

public class PropertyImageService : IPropertyImageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRepository<PropertyImage> _propertyImageRepository;
    private readonly IRepository<EProperty> _epropertyRepository;

    public PropertyImageService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _propertyImageRepository = _unitOfWork.GetRepository<PropertyImage>();
        _epropertyRepository = _unitOfWork.GetRepository<EProperty>();
    }

    public async Task<ResponseDto<PropertyImageDto>> GetAsync(int id)
    {
        try
        {
            var propertyImage = await _propertyImageRepository.GetAsync(
                predicate: x => x.Id == id && !x.IsDeleted,
                includes: q => q.Include(x => x.Property)
            );

            if (propertyImage == null)
            {
                return ResponseDto<PropertyImageDto>.Fail("Emlak resmi bulunamadı", 404);
            }

            var result = _mapper.Map<PropertyImageDto>(propertyImage);
            return ResponseDto<PropertyImageDto>.Success(result, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<PropertyImageDto>.Fail($"Emlak resmi getirilirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<IEnumerable<PropertyImageDto>>> GetAllAsync(
        Expression<Func<PropertyImage, bool>>? predicate = null,
        Func<IQueryable<PropertyImage>, IOrderedQueryable<PropertyImage>>? orderBy = null,
        int? propertyId = null,
        bool? isDeleted = null)
    {
        try
        {
            // Base predicate - soft delete kontrolü
            Expression<Func<PropertyImage, bool>> basePredicate = x => !x.IsDeleted;
            
            // Ek filtreler varsa birleştir
            if (predicate != null)
            {
                basePredicate = basePredicate.And(predicate);
            }

            if (propertyId.HasValue)
            {
                basePredicate = basePredicate.And(x => x.PropertyId == propertyId.Value);
            }

            if (isDeleted.HasValue)
            {
                if (isDeleted.Value)
                {
                    basePredicate = x => x.IsDeleted;
                }
            }

            var propertyImages = await _propertyImageRepository.GetAllAsync(
                predicate: basePredicate,
                orderBy: orderBy ?? (q => q.OrderBy(x => x.DisplayOrder).ThenBy(x => x.CreatedAt)),
                showIsDeleted: isDeleted ?? false,
                includes: Array.Empty<Func<IQueryable<PropertyImage>, IQueryable<PropertyImage>>>()
            );

            var result = _mapper.Map<IEnumerable<PropertyImageDto>>(propertyImages);
            return ResponseDto<IEnumerable<PropertyImageDto>>.Success(result, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<IEnumerable<PropertyImageDto>>.Fail($"Emlak resimleri getirilirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<IEnumerable<PropertyImageDto>>> GetByPropertyIdAsync(int propertyId)
    {
        try
        {
            // Property kontrolü
            var propertyExists = await _epropertyRepository.ExistsAsync(x => x.Id == propertyId && !x.IsDeleted);
            if (!propertyExists)
            {
                return ResponseDto<IEnumerable<PropertyImageDto>>.Fail("Emlak ilanı bulunamadı", 404);
            }

            var propertyImages = await _propertyImageRepository.GetAllAsync(
                predicate: x => x.PropertyId == propertyId && !x.IsDeleted,
                orderBy: q => q.OrderBy(x => x.DisplayOrder).ThenBy(x => x.CreatedAt),
                showIsDeleted: false,
                includes: Array.Empty<Func<IQueryable<PropertyImage>, IQueryable<PropertyImage>>>()
            );

            var result = _mapper.Map<IEnumerable<PropertyImageDto>>(propertyImages);
            return ResponseDto<IEnumerable<PropertyImageDto>>.Success(result, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<IEnumerable<PropertyImageDto>>.Fail($"Emlak resimleri getirilirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<int>> CountAsync()
    {
        try
        {
            var count = await _propertyImageRepository.CountAsync(x => !x.IsDeleted);
            return ResponseDto<int>.Success(count, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<int>.Fail($"Emlak resmi sayısı alınırken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<int>> CountByPropertyIdAsync(int propertyId)
    {
        try
        {
            // Property kontrolü
            var propertyExists = await _epropertyRepository.ExistsAsync(x => x.Id == propertyId && !x.IsDeleted);
            if (!propertyExists)
            {
                return ResponseDto<int>.Fail("Emlak ilanı bulunamadı", 404);
            }

            var count = await _propertyImageRepository.CountAsync(x => x.PropertyId == propertyId && !x.IsDeleted);
            return ResponseDto<int>.Success(count, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<int>.Fail($"Emlak resmi sayısı alınırken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<PropertyImageDto>> CreateAsync(PropertyImageCreateDto propertyImageCreateDto)
    {
        try
        {
            // Property kontrolü
            var propertyExists = await _epropertyRepository.ExistsAsync(x => x.Id == propertyImageCreateDto.PropertyId && !x.IsDeleted);
            if (!propertyExists)
            {
                return ResponseDto<PropertyImageDto>.Fail("Geçersiz emlak ilanı", 400);
            }

            // Eğer ana resim olarak işaretlenmişse, diğer ana resimleri kaldır
            if (propertyImageCreateDto.IsPrimary)
            {
                var existingPrimaryImages = await _propertyImageRepository.GetAllAsync(
                    predicate: x => x.PropertyId == propertyImageCreateDto.PropertyId && x.IsPrimary && !x.IsDeleted,
                    showIsDeleted: false,
                    includes: Array.Empty<Func<IQueryable<PropertyImage>, IQueryable<PropertyImage>>>()
                );

                foreach (var img in existingPrimaryImages)
                {
                    img.IsPrimary = false;
                    img.UpdatedAt = DateTimeOffset.UtcNow;
                    _propertyImageRepository.Update(img);
                }
            }

            var entity = _mapper.Map<PropertyImage>(propertyImageCreateDto);
            entity.CreatedAt = DateTimeOffset.UtcNow;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            entity.IsDeleted = false;

            await _propertyImageRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<PropertyImageDto>(entity);
            return ResponseDto<PropertyImageDto>.Success(result, 201);
        }
        catch (Exception ex)
        {
            return ResponseDto<PropertyImageDto>.Fail($"Emlak resmi oluşturulurken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> UpdateAsync(int id, PropertyImageUpdateDto propertyImageUpdateDto)
    {
        try
        {
            var propertyImage = await _propertyImageRepository.GetAsync(x => x.Id == id && !x.IsDeleted);

            if (propertyImage == null)
            {
                return ResponseDto<NoContent>.Fail("Emlak resmi bulunamadı", 404);
            }

            // Eğer ana resim olarak işaretlenmişse, diğer ana resimleri kaldır
            if (propertyImageUpdateDto.IsPrimary.HasValue && propertyImageUpdateDto.IsPrimary.Value)
            {
                var existingPrimaryImages = await _propertyImageRepository.GetAllAsync(
                    predicate: x => x.PropertyId == propertyImage.PropertyId && x.IsPrimary && x.Id != id && !x.IsDeleted,
                    showIsDeleted: false,
                    includes: Array.Empty<Func<IQueryable<PropertyImage>, IQueryable<PropertyImage>>>()
                );

                foreach (var img in existingPrimaryImages)
                {
                    img.IsPrimary = false;
                    img.UpdatedAt = DateTimeOffset.UtcNow;
                    _propertyImageRepository.Update(img);
                }
            }

            _mapper.Map(propertyImageUpdateDto, propertyImage);
            propertyImage.UpdatedAt = DateTimeOffset.UtcNow;

            _propertyImageRepository.Update(propertyImage);
            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Emlak resmi güncellenirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> SetAsPrimaryAsync(int id, int propertyId)
    {
        try
        {
            var propertyImage = await _propertyImageRepository.GetAsync(x => x.Id == id && x.PropertyId == propertyId && !x.IsDeleted);

            if (propertyImage == null)
            {
                return ResponseDto<NoContent>.Fail("Emlak resmi bulunamadı", 404);
            }

            // Diğer ana resimleri kaldır
            var existingPrimaryImages = await _propertyImageRepository.GetAllAsync(
                predicate: x => x.PropertyId == propertyId && x.IsPrimary && x.Id != id && !x.IsDeleted,
                showIsDeleted: false,
                includes: Array.Empty<Func<IQueryable<PropertyImage>, IQueryable<PropertyImage>>>()
            );

            foreach (var img in existingPrimaryImages)
            {
                img.IsPrimary = false;
                img.UpdatedAt = DateTimeOffset.UtcNow;
                _propertyImageRepository.Update(img);
            }

            // Bu resmi ana resim yap
            propertyImage.IsPrimary = true;
            propertyImage.UpdatedAt = DateTimeOffset.UtcNow;
            _propertyImageRepository.Update(propertyImage);

            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Ana resim ayarlanırken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> UpdateDisplayOrderAsync(int id, int displayOrder)
    {
        try
        {
            var propertyImage = await _propertyImageRepository.GetAsync(x => x.Id == id && !x.IsDeleted);

            if (propertyImage == null)
            {
                return ResponseDto<NoContent>.Fail("Emlak resmi bulunamadı", 404);
            }

            propertyImage.DisplayOrder = displayOrder;
            propertyImage.UpdatedAt = DateTimeOffset.UtcNow;

            _propertyImageRepository.Update(propertyImage);
            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Resim sırası güncellenirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> SoftDeleteAsync(int id)
    {
        try
        {
            var propertyImage = await _propertyImageRepository.GetAsync(x => x.Id == id && !x.IsDeleted);

            if (propertyImage == null)
            {
                return ResponseDto<NoContent>.Fail("Emlak resmi bulunamadı", 404);
            }

            propertyImage.IsDeleted = true;
            propertyImage.UpdatedAt = DateTimeOffset.UtcNow;

            _propertyImageRepository.Update(propertyImage);
            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Emlak resmi silinirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> HardDeleteAsync(int id)
    {
        try
        {
            var propertyImage = await _propertyImageRepository.GetAsync(x => x.Id == id);

            if (propertyImage == null)
            {
                return ResponseDto<NoContent>.Fail("Emlak resmi bulunamadı", 404);
            }

            _propertyImageRepository.Remove(propertyImage);
            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Emlak resmi kalıcı olarak silinirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> DeleteAllByPropertyIdAsync(int propertyId)
    {
        try
        {
            // Property kontrolü
            var propertyExists = await _epropertyRepository.ExistsAsync(x => x.Id == propertyId);
            if (!propertyExists)
            {
                return ResponseDto<NoContent>.Fail("Emlak ilanı bulunamadı", 404);
            }

            var propertyImages = await _propertyImageRepository.GetAllAsync(
                predicate: x => x.PropertyId == propertyId && !x.IsDeleted,
                showIsDeleted: false,
                includes: Array.Empty<Func<IQueryable<PropertyImage>, IQueryable<PropertyImage>>>()
            );

            foreach (var img in propertyImages)
            {
                img.IsDeleted = true;
                img.UpdatedAt = DateTimeOffset.UtcNow;
                _propertyImageRepository.Update(img);
            }

            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Emlak resimleri silinirken hata oluştu: {ex.Message}", 500);
        }
    }
}