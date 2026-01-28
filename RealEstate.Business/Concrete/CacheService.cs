using System;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using RealEstate.Business.Abstract;

namespace RealEstate.Business.Concrete;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(30);

    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        try
        {
            if (_memoryCache.TryGetValue(key, out var cachedValue))
            {
                if (cachedValue is string jsonString)
                {
                    return JsonSerializer.Deserialize<T>(jsonString);
                }
                return cachedValue as T;
            }
            return null;
        }
        catch (Exception ex)
        {
            // Log the exception but don't throw - cache failures shouldn't break the app
            Console.WriteLine($"Cache get error for key {key}: {ex.Message}");
            return null;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
    {
        try
        {
            var cacheExpiration = expiration ?? _defaultExpiration;
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheExpiration,
                SlidingExpiration = TimeSpan.FromMinutes(5), // Sliding expiration
                Priority = CacheItemPriority.Normal
            };

            var jsonString = JsonSerializer.Serialize(value);
            _memoryCache.Set(key, jsonString, options);
        }
        catch (Exception ex)
        {
            // Log the exception but don't throw - cache failures shouldn't break the app
            Console.WriteLine($"Cache set error for key {key}: {ex.Message}");
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            _memoryCache.Remove(key);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cache remove error for key {key}: {ex.Message}");
        }
    }

    public async Task RemoveByPatternAsync(string pattern)
    {
        try
        {
            // Memory cache doesn't support pattern-based removal directly
            // This is a simplified implementation
            if (_memoryCache is MemoryCache memCache)
            {
                var field = typeof(MemoryCache).GetField("_coherentState", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (field?.GetValue(memCache) is object coherentState)
                {
                    var entriesCollection = coherentState.GetType()
                        .GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    
                    if (entriesCollection?.GetValue(coherentState) is IDictionary entries)
                    {
                        var keysToRemove = new List<object>();
                        foreach (DictionaryEntry entry in entries)
                        {
                            if (entry.Key.ToString()?.Contains(pattern) == true)
                            {
                                keysToRemove.Add(entry.Key);
                            }
                        }

                        foreach (var key in keysToRemove)
                        {
                            _memoryCache.Remove(key);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cache remove by pattern error for pattern {pattern}: {ex.Message}");
        }
    }

    public async Task<bool> ExistsAsync(string key)
    {
        try
        {
            return _memoryCache.TryGetValue(key, out _);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cache exists check error for key {key}: {ex.Message}");
            return false;
        }
    }
}