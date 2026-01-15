using System;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Data.Abstract;

namespace RealEstate.Data.Concrete;

public class UnitOfWork : IUnitOfWork
{
   private readonly RealEstateDbContext _context;

   private readonly IServiceProvider _serviceProvider;

   private  IDbContextTransaction _transaction;

    public UnitOfWork(RealEstateDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public async Task BeginTransactionAsync()
    {
         _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
       try
       {
          await _context.SaveChangesAsync();
          await _transaction.CommitAsync();
       }
       catch
       {
          await RollbackTransactionAsync();
       }
       finally
        {
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }

    public IRepository<T> GetRepository<T>() where T : class
    {
       var repository = _serviceProvider.GetRequiredService<IRepository<T>>();
       return repository;
    }

    public async Task RollbackTransactionAsync()
    {
         if (_transaction is not null)
        {
            await _transaction.RollbackAsync();
        }
    }

    public async Task<int> SaveAsync()
    {
        var result = await _context.SaveChangesAsync();
        return result;
    }
}
