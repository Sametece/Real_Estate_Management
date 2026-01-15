using System;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data.Abstract;

namespace RealEstate.Data.Concrete;

public class Repository<T> : IRepository<T> where T : class
{
   protected readonly RealEstateDbContext _context;

   private readonly DbSet<T> _dbset;

    public Repository(RealEstateDbContext context, DbSet<T> dbset)
    {
        _context = context;
        _dbset = _context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
         await _dbset.AddAsync(entity);
    }

    public void BatchUpdate(IEnumerable<T> entities)
    {
        _dbset.UpdateRange(entities);
    }

    public async Task<int> CountAsync()
    {
        var result = await _dbset.CountAsync();
        return result;
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        var result = await _dbset.CountAsync(predicate);
        return result ;
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
       var result = await _dbset.AnyAsync(predicate);
       return result ;

    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var result = await _dbset.ToListAsync();
        return result ;
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>> predicate = null!,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
        bool showIsDeleted = false,
        bool asExpanded = false,
        params Func<IQueryable<T>, IQueryable<T>>[] includes)
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

    public async Task<T> GetAsync(int id)
    {
        var result = await _dbset.FindAsync(id);
        return result!;
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool showIsDeleted = false, bool asExpanded = false, params Func<IQueryable<T>, IQueryable<T>>[] includes)
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

    public async Task<(IEnumerable<T> Data, int TotalCount)> GetPagedAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int skip = 0, int take = 10, bool showIsDeleted = false, bool asExpanded = false, params Func<IQueryable<T>, IQueryable<T>>[] includes)
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

    public void Remove(T entity)
    {
       _dbset.Remove(entity);
    }

    public void Update(T entity)
    {
        _dbset.Update(entity);
    }
}
