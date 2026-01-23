using System;
using System.Linq.Expressions;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.Inquiry;
using RealEstate.Business.DTOs.ResponseDto;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Abstract;

public interface IInguiryService
{
    /// <summary>
    /// id'li mesajı getir
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ResponseDto<InquiryListDto>> GetAsync(int id);
    
    /// <summary>
    /// Tüm mesajları getir
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="orderBy"></param>
    /// <param name="includeEProperty"></param>
    /// <param name="epropertyId"></param>
    /// <param name="isDeleted"></param>
    /// <returns></returns>
    Task<ResponseDto<IEnumerable<InquiryListDto>>> GetAllAsync(
        Expression<Func<EProperty, bool>>? predicate,
        Func<IQueryable<EProperty>, IOrderedQueryable<EProperty>>? orderBy,
        bool? includeEProperty = null,
        int? epropertyId = null,
        bool? isDeleted = null
    );

    
    /// <summary>
    /// Yeni mesaj yarat
    /// </summary>
    /// <param name="inquiryCreateDto"></param>
    /// <returns></returns>
   Task<ResponseDto<InquiryCreateDto>> InquiryCreateAsync(InquiryCreateDto inquiryCreateDto);

   /// <summary>
   /// Mesajları sayfalı ekranda getir
   /// </summary>
   /// <param name="paginationQueryDto"></param>
   /// <param name="predicate"></param>
   /// <param name="orderBy"></param>
   /// <param name="includeEProperty"></param>
   /// <param name="epropertyId"></param>
   /// <param name="isDeleted"></param>
   /// <returns></returns>
   Task<ResponseDto<EPropertyPagedResultDto<EPropertyListDto>>> GetAllPagedAsync(
        EPropertyPaginationQueryDto paginationQueryDto,
        Expression<Func<EProperty, bool>>? predicate = null,
        Func<IQueryable<EProperty>, IOrderedQueryable<EProperty>>? orderBy = null,
        bool? includeEProperty = null,
        int? epropertyId = null,
        bool? isDeleted = null);
     
     /// <summary>
     /// Mesaj Sayısını gör
     /// </summary>
     /// <returns></returns>
    Task<ResponseDto<int>> CountAsync();
    
    /// <summary>
    /// Admin mesaj durumunu güncelller
    /// </summary>
    /// <param name="inquiryAdminUpdateDto"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> UpdateAsync(InquiryAdminUpdateDto inquiryAdminUpdateDto);

    Task<ResponseDto<NoContent>> SoftDeleteAsync(int id);

    Task<ResponseDto<NoContent>> HardDeleteAsync(int id);


}
