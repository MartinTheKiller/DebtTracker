using AutoMapper;
using CubiTracker.DAL.Repositories;
using DebtTracker.BL.Hashers;
using DebtTracker.BL.Models;
using DebtTracker.BL.Models.User;
using DebtTracker.DAL.Entities;
using DebtTracker.DAL.Mappers;
using DebtTracker.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.BL.Facades;

public class UserFacade : FacadeBase<UserEntity, UserListModel, UserDetailModel, UserCreateModel, UserDetailModel, UserEntityMapper>, IUserFacade
{
    private readonly IUserPasswordHasher _passwordHasher;

    public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper modelMapper, IUserPasswordHasher passwordHasher) : base(unitOfWorkFactory, modelMapper)
    {
        _passwordHasher = passwordHasher;
    }

    public new async Task<UserDetailModel> UpdateAsync(UserDetailModel model)
    {
        GuardCollectionsAreNotSet(model);

        UserEntity entity = ModelMapper.Map<UserEntity>(model);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<UserEntity> repository = uow.GetRepository<UserEntity, UserEntityMapper>();

        if (!await repository.ExistsAsync(entity))
            throw new InvalidOperationException("Entity does not exists.");

        IQueryable<UserEntity> query = repository.Get().Where(e => e.Id == entity.Id);
        entity.HashedPassword = await query.Select(e => e.HashedPassword).SingleAsync();

        UserEntity updatedEntity = await repository.UpdateAsync(entity);

        await uow.CommitAsync();

        return ModelMapper.Map<UserDetailModel>(updatedEntity);
    }

    public async Task<Guid?> LoginAsync(string email, string password)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<UserEntity> query = uow
            .GetRepository<UserEntity, UserEntityMapper>()
            .Get()
            .Where(e => e.Email == email);

        UserPasswordModel? userPasswordModel = await ModelMapper.ProjectTo<UserPasswordModel>(query).SingleOrDefaultAsync().ConfigureAwait(false);

        if (userPasswordModel is null) return null;

        bool passwordMatches = _passwordHasher.VerifyHashedPassword(password, userPasswordModel.HashedPassword);

        return passwordMatches ? userPasswordModel.Id : null;
    }
}