using System;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Data.Abstract;

namespace RealEstate.Data.Concrete;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly RealEstateDbContext _context;
    private readonly DbSet<T> _dbset;

    public Repository(RealEstateDbContext context)
    {
        _context = context;
        _dbset = _context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        try
        {
            await _dbset.AddAsync(entity);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Entity eklenirken hata oluştu: {ex.Message}", ex);
        }
    }

    public void BatchUpdate(IEnumerable<T> entities)
    {
        try
        {
            _dbset.UpdateRange(entities);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Toplu güncelleme yapılırken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task<int> CountAsync()
    {
        try
        {
            var result = await _dbset.CountAsync();
            return result;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Kayıt sayısı alınırken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        try
        {
            var result = await _dbset.CountAsync(predicate);
            return result;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Filtrelenmiş kayıt sayısı alınırken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        try
        {
            var result = await _dbset.AnyAsync(predicate);
            return result;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Kayıt varlığı kontrol edilirken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            var result = await _dbset.ToListAsync();
            return result;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Tüm kayıtlar getirilirken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>> predicate = null!,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
        bool showIsDeleted = false,
        bool asExpanded = false,
        params Func<IQueryable<T>, IQueryable<T>>[] includes)
    {
        try
        {
            IQueryable<T> query = _dbset;
            
            if (showIsDeleted)
            {
                query = query.IgnoreQueryFilters();
            }

            if (asExpanded)
            {
                query = query.AsExpandable();
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (includes != null && includes.Length > 0)
            {
                query = includes.Aggregate(query, (current, include) => include(current));
            }
          
            var result = await query.ToListAsync();
            return result;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Filtrelenmiş kayıtlar getirilirken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task<T> GetAsync(int id)
    {
        try
        {
            var result = await _dbset.FindAsync(id);
            return result!;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"ID ile kayıt getirilirken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task<T> GetByIdAsync(int id)
    {
        try
        {
            var result = await _dbset.FindAsync(id);
            return result!;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"ID ile kayıt getirilirken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool showIsDeleted = false, bool asExpanded = false, params Func<IQueryable<T>, IQueryable<T>>[] includes)
    {
        try
        {
            IQueryable<T> query = _dbset;
            
            if (showIsDeleted)
            {
                query = query.IgnoreQueryFilters();
            }

            if (asExpanded)
            {
                query = query.AsExpandable();
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => include(current));
            }

            var result = await query.FirstOrDefaultAsync();
            return result!;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Predicate ile kayıt getirilirken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task<(IEnumerable<T> Data, int TotalCount)> GetPagedAsync(
        Expression<Func<T, bool>>? predicate = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, 
        int skip = 0, 
        int take = 10, 
        bool showIsDeleted = false, 
        bool asExpanded = false, 
        params Func<IQueryable<T>, IQueryable<T>>[] includes)
    {
        try
        {
            IQueryable<T> query = _dbset;
            
            if (showIsDeleted)
            {
                query = query.IgnoreQueryFilters();
            }

            if (asExpanded)
            {
                query = query.AsExpandable();
            }
            
            if (predicate is not null)
            {
                query = query.Where(predicate);
            }
            
            var totalCount = await query.CountAsync();
            
            if (orderBy is not null)
            {
                query = orderBy(query);
            }

            query = query.Skip(skip).Take(take);

            if (includes is not null && includes.Length > 0)
            {
                query = includes.Aggregate(query, (current, include) => include(current));
            }
            
            var data = await query.ToListAsync();
            return (data, totalCount);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Sayfalanmış kayıtlar getirilirken hata oluştu: {ex.Message}", ex);
        }
    }

    public void Remove(T entity)
    {
        try
        {
            _dbset.Remove(entity);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Entity silinirken hata oluştu: {ex.Message}", ex);
        }
    }

    public void Update(T entity)
    {
        try
        {
            _dbset.Update(entity);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Entity güncellenirken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task GetByIdAsync(int id, string[] includes)
    {
        try
        {
            IQueryable<T> query = _dbset;
            
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            
            var result = await query.FirstOrDefaultAsync();
            // Bu metod void döndürüyor interface'de, muhtemelen hatalı tanımlanmış
            // Gerçek implementasyonda T döndürmeli
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"ID ve includes ile kayıt getirilirken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task UpdateAsync(T entity)
    {
        try
        {
            _dbset.Update(entity);
            await Task.CompletedTask; // Update synchronous bir işlem, async wrapper
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Entity güncellenirken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task DeleteAsync(T entity)
    {
        try
        {
            _dbset.Remove(entity);
            await Task.CompletedTask; // Remove synchronous bir işlem, async wrapper
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Entity silinirken hata oluştu: {ex.Message}", ex);
        }
    }
}
