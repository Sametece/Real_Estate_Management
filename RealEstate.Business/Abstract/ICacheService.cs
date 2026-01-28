using System;

namespace RealEstate.Business.Abstract;

public interface ICacheService
{
    /// <summary>
    /// Cache'den veri al
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<T?> GetAsync<T>(string key) where T : class;

    /// <summary>
    /// Cache'e veri ekle
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiration"></param>
    /// <returns></returns>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class;

    /// <summary>
    /// Cache'den veri sil
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task RemoveAsync(string key);

    /// <summary>
    /// Pattern'e göre cache'leri sil
    /// </summary>
    /// <param name="pattern"></param>
    /// <returns></returns>
    Task RemoveByPatternAsync(string pattern);

    /// <summary>
    /// Cache'de veri var mı kontrol et
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(string key);
}