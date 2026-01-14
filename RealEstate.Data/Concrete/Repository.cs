using System;
using System.Linq.Expressions;
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

    public Task AddAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public void BatchUpdate(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync()
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var result = await _dbset.ToListAsync();
        return result ;
    }

    public Task<T> GetAllAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, bool showIsDeleted = false, bool asExpanded = false, params Func<IQueryable<T>, IQueryable<T>>[] includes)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool showIsDeleted = false, bool asExpanded = false, params Func<IQueryable<T>, IQueryable<T>>[] includes)
    {
        throw new NotImplementedException();
    }

    public Task<(IEnumerable<T> Data, int TotalCount)> GetPagedAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int skip = 0, int take = 10, bool showIsDeleted = false, bool asExpanded = false, params Func<IQueryable<T>, IQueryable<T>>[] includes)
    {
        throw new NotImplementedException();
    }

    public void Remove(T entity)
    {
        throw new NotImplementedException();
    }

    public void Update(T entity)
    {
        throw new NotImplementedException();
    }
}
