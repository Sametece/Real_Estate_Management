using System;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Data.Abstract;

namespace RealEstate.Data.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly RealEstateDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private IDbContextTransaction _transaction;

    public UnitOfWork(RealEstateDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public async Task BeginTransactionAsync()
    {
        try
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Transaction başlatılırken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await RollbackTransactionAsync();
            throw new InvalidOperationException($"Transaction commit edilirken hata oluştu: {ex.Message}", ex);
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null!;
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
            await _context.DisposeAsync();
        }
        catch (Exception ex)
        {
            // Dispose işleminde hata olsa bile devam etmeli
            // Log edilebilir ama exception fırlatılmamalı
            Console.WriteLine($"UnitOfWork dispose edilirken hata oluştu: {ex.Message}");
        }
    }

    public IRepository<T> GetRepository<T>() where T : class
    {
        try
        {
            var repository = _serviceProvider.GetRequiredService<IRepository<T>>();
            return repository;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Repository alınırken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync();
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Transaction rollback edilirken hata oluştu: {ex.Message}", ex);
        }
    }

    public async Task<int> SaveAsync()
    {
        try
        {
            var result = await _context.SaveChangesAsync();
            return result;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Değişiklikler kaydedilirken hata oluştu: {ex.Message}", ex);
        }
    }
}
