using System.Collections;
using AutoMapper;
using DebtTracker.BL.Models;
using DebtTracker.DAL.Entities;
using DebtTracker.DAL.Mappers;
using DebtTracker.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using CubiTracker.DAL.Repositories;

namespace DebtTracker.BL.Facades;

public abstract class FacadeBase<TEntity, TListModel, TDetailModel, TCreateModel, TUpdateModel, TEntityMapper> : IFacade<TEntity, TListModel, TDetailModel, TCreateModel, TUpdateModel>
    where TEntity : class, IEntity
    where TListModel : class, IModel
    where TDetailModel : class, IModel
    where TCreateModel : class, IModel
    where TUpdateModel : class, IModel
    where TEntityMapper : IEntityMapper<TEntity>, new()
{
    protected readonly IMapper ModelMapper;
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory;

    protected FacadeBase(
        IUnitOfWorkFactory unitOfWorkFactory,
        IMapper modelMapper)
    {
        ModelMapper = modelMapper;
        UnitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task DeleteAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        uow.GetRepository<TEntity, TEntityMapper>().Delete(id);
        await uow.CommitAsync().ConfigureAwait(false);
    }

    public async Task<TDetailModel?> GetAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<TEntity> query = uow
            .GetRepository<TEntity, TEntityMapper>()
            .Get()
            .Where(e => e.Id == id);
        return await ModelMapper.ProjectTo<TDetailModel>(query).SingleOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<TListModel>> GetAsync()
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<TEntity> query = uow
            .GetRepository<TEntity, TEntityMapper>()
            .Get();
        return await ModelMapper.ProjectTo<TListModel>(query).ToListAsync().ConfigureAwait(false);
    }

    public async Task<TDetailModel> CreateAsync(TCreateModel model)
    {
        GuardCollectionsAreNotSet(model);

        TEntity entity = ModelMapper.Map<TEntity>(model);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<TEntity> repository = uow.GetRepository<TEntity, TEntityMapper>();

        if (await repository.ExistsAsync(entity))
            throw new InvalidOperationException("Entity already exists.");

        entity.Id = entity.Id == Guid.Empty ? Guid.NewGuid() : entity.Id;
        TEntity insertedEntity = await repository.InsertAsync(entity);

        await uow.CommitAsync();

        return ModelMapper.Map<TDetailModel>(insertedEntity);
    }

    public async Task<TDetailModel> UpdateAsync(TUpdateModel model)
    {
        GuardCollectionsAreNotSet(model);

        TEntity entity = ModelMapper.Map<TEntity>(model);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<TEntity> repository = uow.GetRepository<TEntity, TEntityMapper>();

        if (!await repository.ExistsAsync(entity))
            throw new InvalidOperationException("Entity does not exists.");

        TEntity updatedEntity = await repository.UpdateAsync(entity);

        await uow.CommitAsync();

        return ModelMapper.Map<TDetailModel>(updatedEntity);
    }

    public virtual async Task<TDetailModel> SaveAsync(TCreateModel model)
    {
        TDetailModel result;
        GuardCollectionsAreNotSet(model);

        TEntity entity = ModelMapper.Map<TEntity>(model);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<TEntity> repository = uow.GetRepository<TEntity, TEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            TEntity updatedEntity = await repository.UpdateAsync(entity);
            result = ModelMapper.Map<TDetailModel>(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            TEntity insertedEntity = await repository.InsertAsync(entity);
            result = ModelMapper.Map<TDetailModel>(insertedEntity);
        }

        await uow.CommitAsync();

        return result;
    }

    /// <summary>
    /// This Guard ensures that there is a clear understanding of current infrastructure limitations.
    /// This version of BL/DAL infrastructure does not support insertion or update of adjacent entities
    /// </summary>
    /// <param name="model">Model to be inserted or updated</param>
    /// <exception cref="InvalidOperationException"></exception>
    protected static void GuardCollectionsAreNotSet(IModel model)
    {
        IEnumerable<PropertyInfo> collectionProperties = model
            .GetType()
            .GetProperties()
            .Where(i => typeof(ICollection).IsAssignableFrom(i.PropertyType));

        foreach (PropertyInfo collectionProperty in collectionProperties)
        {
            if (collectionProperty.GetValue(model) is ICollection { Count: > 0 })
            {
                throw new InvalidOperationException(
                    "Current BL and DAL infrastructure disallows insert or update of models with adjacent collections.");
            }
        }
    }
}