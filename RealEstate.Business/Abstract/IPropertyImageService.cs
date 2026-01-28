using System;
using System.Linq.Expressions;
using RealEstate.Business.DTOs;
using RealEstate.Business.DTOs.ResponseDto;
using RealEstate.Entity.Concrete;

namespace RealEstate.Business.Abstract;

public interface IPropertyImageService
{
    /// <summary>
    /// ID'ye göre emlak resmi getir
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ResponseDto<PropertyImageDto>> GetAsync(int id);
    
    /// <summary>
    /// Tüm emlak resimlerini getir
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="orderBy"></param>
    /// <param name="propertyId"></param>
    /// <param name="isDeleted"></param>
    /// <returns></returns>
    Task<ResponseDto<IEnumerable<PropertyImageDto>>> GetAllAsync(
        Expression<Func<PropertyImage, bool>>? predicate = null,
        Func<IQueryable<PropertyImage>, IOrderedQueryable<PropertyImage>>? orderBy = null,
        int? propertyId = null,
        bool? isDeleted = null
    );

    /// <summary>
    /// Belirli bir emlak ilanının resimlerini getir
    /// </summary>
    /// <param name="propertyId"></param>
    /// <returns></returns>
    Task<ResponseDto<IEnumerable<PropertyImageDto>>> GetByPropertyIdAsync(int propertyId);

    /// <summary>
    /// Emlak resmi sayısını getir
    /// </summary>
    /// <returns></returns>
    Task<ResponseDto<int>> CountAsync();

    /// <summary>
    /// Belirli bir emlak ilanının resim sayısını getir
    /// </summary>
    /// <param name="propertyId"></param>
    /// <returns></returns>
    Task<ResponseDto<int>> CountByPropertyIdAsync(int propertyId);

    /// <summary>
    /// Yeni emlak resmi oluştur
    /// </summary>
    /// <param name="propertyImageCreateDto"></param>
    /// <returns></returns>
    Task<ResponseDto<PropertyImageDto>> CreateAsync(PropertyImageCreateDto propertyImageCreateDto);

    /// <summary>
    /// Emlak resmini güncelle
    /// </summary>
    /// <param name="id"></param>
    /// <param name="propertyImageUpdateDto"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> UpdateAsync(int id, PropertyImageUpdateDto propertyImageUpdateDto);

    /// <summary>
    /// Ana resim olarak ayarla
    /// </summary>
    /// <param name="id"></param>
    /// <param name="propertyId"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> SetAsPrimaryAsync(int id, int propertyId);

    /// <summary>
    /// Resim sırasını güncelle
    /// </summary>
    /// <param name="id"></param>
    /// <param name="displayOrder"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> UpdateDisplayOrderAsync(int id, int displayOrder);

    /// <summary>
    /// Emlak resmini soft delete
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> SoftDeleteAsync(int id);

    /// <summary>
    /// Emlak resmini kalıcı olarak sil
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> HardDeleteAsync(int id);

    /// <summary>
    /// Belirli bir emlak ilanının tüm resimlerini sil
    /// </summary>
    /// <param name="propertyId"></param>
    /// <returns></returns>
    Task<ResponseDto<NoContent>> DeleteAllByPropertyIdAsync(int propertyId);
}