using CubiTracker.DAL.Repositories;
using DebtTracker.DAL.Entities;
using DebtTracker.DAL.Mappers;

namespace DebtTracker.DAL.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
        where TEntity : class, IEntity
        where TEntityMapper : IEntityMapper<TEntity>, new();
    Task CommitAsync();
}