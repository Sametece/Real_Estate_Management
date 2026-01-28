using System;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealEstate.Business.Abstract;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.Inquiry;
using RealEstate.Business.DTOs.ResponseDto;
using RealEstate.Business.Extensions;
using RealEstate.Data.Abstract;
using RealEstate.Entity.Concrete;
using RealEstate.Entity.Enum;

namespace RealEstate.Business.Concrete;

public class InquiryService : IInguiryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRepository<Inquiry> _inquiryRepository;
    private readonly IRepository<EProperty> _epropertyRepository;

    public InquiryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _inquiryRepository = _unitOfWork.GetRepository<Inquiry>();
        _epropertyRepository = _unitOfWork.GetRepository<EProperty>();
    }

    public async Task<ResponseDto<InquiryListDto>> GetAsync(int id)
    {
        try
        {
            var inquiry = await _inquiryRepository.GetAsync(
                predicate: x => x.Id == id && !x.IsDeleted,
                includes: q => q.Include(x => x.Property)
            );

            if (inquiry == null)
            {
                return ResponseDto<InquiryListDto>.Fail("Mesaj bulunamadı", 404);
            }

            var result = _mapper.Map<InquiryListDto>(inquiry);
            return ResponseDto<InquiryListDto>.Success(result, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<InquiryListDto>.Fail($"Mesaj getirilirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<IEnumerable<InquiryListDto>>> GetAllAsync(
        Expression<Func<EProperty, bool>>? predicate,
        Func<IQueryable<EProperty>, IOrderedQueryable<EProperty>>? orderBy,
        bool? includeEProperty = null,
        int? epropertyId = null,
        bool? isDeleted = null)
    {
        try
        {
            // Inquiry için base predicate
            Expression<Func<Inquiry, bool>> inquiryPredicate = x => !x.IsDeleted;

            if (epropertyId.HasValue)
            {
                inquiryPredicate = inquiryPredicate.And(x => x.PropertyId == epropertyId.Value);
            }

            if (isDeleted.HasValue)
            {
                if (isDeleted.Value)
                {
                    inquiryPredicate = x => x.IsDeleted;
                }
            }

            var inquiries = await _inquiryRepository.GetAllAsync(
                predicate: inquiryPredicate,
                showIsDeleted: isDeleted ?? false,
                includes: includeEProperty == true ? 
                    new Func<IQueryable<Inquiry>, IQueryable<Inquiry>>[] { q => q.Include(x => x.Property) } :
                    Array.Empty<Func<IQueryable<Inquiry>, IQueryable<Inquiry>>>()
            );

            var result = _mapper.Map<IEnumerable<InquiryListDto>>(inquiries);
            return ResponseDto<IEnumerable<InquiryListDto>>.Success(result, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<IEnumerable<InquiryListDto>>.Fail($"Mesajlar getirilirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<InquiryCreateDto>> InquiryCreateAsync(InquiryCreateDto inquiryCreateDto)
    {
        try
        {
            // Property kontrolü
            var propertyExists = await _epropertyRepository.ExistsAsync(x => x.Id == inquiryCreateDto.PropertyId && !x.IsDeleted);
            if (!propertyExists)
            {
                return ResponseDto<InquiryCreateDto>.Fail("Geçersiz emlak ilanı", 400);
            }

            var entity = _mapper.Map<Inquiry>(inquiryCreateDto);
            entity.CreatedAt = DateTimeOffset.UtcNow;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            entity.IsDeleted = false;
            entity.Status = InquiryStatus.New;

            await _inquiryRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<InquiryCreateDto>(entity);
            return ResponseDto<InquiryCreateDto>.Success(result, 201);
        }
        catch (Exception ex)
        {
            return ResponseDto<InquiryCreateDto>.Fail($"Mesaj oluşturulurken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<EPropertyPagedResultDto<EPropertyListDto>>> GetAllPagedAsync(
        EPropertyPaginationQueryDto paginationQueryDto,
        Expression<Func<EProperty, bool>>? predicate = null,
        Func<IQueryable<EProperty>, IOrderedQueryable<EProperty>>? orderBy = null,
        bool? includeEProperty = null,
        int? epropertyId = null,
        bool? isDeleted = null)
    {
        try
        {
            // Bu metod Inquiry için değil, EProperty için sayfalama yapıyor
            // Interface'de yanlış tanımlanmış gibi görünüyor, ama interface'e uygun implement ediyorum
            
            Expression<Func<EProperty, bool>> basePredicate = x => !x.IsDeleted;
            
            if (predicate != null)
            {
                basePredicate = basePredicate.And(predicate);
            }

            if (epropertyId.HasValue)
            {
                basePredicate = basePredicate.And(x => x.Id == epropertyId.Value);
            }

            if (isDeleted.HasValue && isDeleted.Value)
            {
                basePredicate = x => x.IsDeleted;
            }

            var (data, totalCount) = await _epropertyRepository.GetPagedAsync(
                predicate: basePredicate,
                orderBy: orderBy,
                skip: paginationQueryDto.Skip,
                take: paginationQueryDto.Take,
                showIsDeleted: isDeleted ?? false,
                includes: includeEProperty == true ? 
                    new Func<IQueryable<EProperty>, IQueryable<EProperty>>[] { q => q.Include(x => x.PropertyType) } :
                    Array.Empty<Func<IQueryable<EProperty>, IQueryable<EProperty>>>()
            );

            var mappedData = _mapper.Map<IEnumerable<EPropertyListDto>>(data);
            var pagedResult = EPropertyPagedResultDto<EPropertyListDto>.Create(
                mappedData,
                paginationQueryDto.PageNumber,
                paginationQueryDto.PageSize,
                totalCount
            );

            return ResponseDto<EPropertyPagedResultDto<EPropertyListDto>>.Success(pagedResult, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<EPropertyPagedResultDto<EPropertyListDto>>.Fail($"Sayfalanmış veriler getirilirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<int>> CountAsync()
    {
        try
        {
            var count = await _inquiryRepository.CountAsync(x => !x.IsDeleted);
            return ResponseDto<int>.Success(count, 200);
        }
        catch (Exception ex)
        {
            return ResponseDto<int>.Fail($"Mesaj sayısı alınırken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> UpdateAsync(InquiryAdminUpdateDto inquiryAdminUpdateDto)
    {
        try
        {
            var inquiry = await _inquiryRepository.GetAsync(x => x.Id == inquiryAdminUpdateDto.Id && !x.IsDeleted);

            if (inquiry == null)
            {
                return ResponseDto<NoContent>.Fail("Mesaj bulunamadı", 404);
            }

            _mapper.Map(inquiryAdminUpdateDto, inquiry);
            inquiry.UpdatedAt = DateTimeOffset.UtcNow;

            _inquiryRepository.Update(inquiry);
            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Mesaj güncellenirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> SoftDeleteAsync(int id)
    {
        try
        {
            var inquiry = await _inquiryRepository.GetAsync(x => x.Id == id && !x.IsDeleted);

            if (inquiry == null)
            {
                return ResponseDto<NoContent>.Fail("Mesaj bulunamadı", 404);
            }

            inquiry.IsDeleted = true;
            inquiry.UpdatedAt = DateTimeOffset.UtcNow;

            _inquiryRepository.Update(inquiry);
            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Mesaj silinirken hata oluştu: {ex.Message}", 500);
        }
    }

    public async Task<ResponseDto<NoContent>> HardDeleteAsync(int id)
    {
        try
        {
            var inquiry = await _inquiryRepository.GetAsync(x => x.Id == id);

            if (inquiry == null)
            {
                return ResponseDto<NoContent>.Fail("Mesaj bulunamadı", 404);
            }

            _inquiryRepository.Remove(inquiry);
            await _unitOfWork.SaveAsync();

            return ResponseDto<NoContent>.Success(204);
        }
        catch (Exception ex)
        {
            return ResponseDto<NoContent>.Fail($"Mesaj kalıcı olarak silinirken hata oluştu: {ex.Message}", 500);
        }
    }
}
