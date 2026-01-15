using System;

namespace RealEstate.Data.Abstract;

public interface IUnitOfWork : IAsyncDisposable
{
  IRepository<T> GetRepository<T>() where T : class ;

  Task<int> SaveAsync();
  Task BeginTransactionAsync();
  Task CommitTransactionAsync();

  Task RollbackTransactionAsync();
}
