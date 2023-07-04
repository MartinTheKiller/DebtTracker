using AutoMapper;
using DebtTracker.BL.Hashers;
using DebtTracker.BL.Models;
using DebtTracker.DAL.Entities;
using DebtTracker.DAL.Mappers;
using DebtTracker.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.BL.Facades;

public class UserFacade : FacadeBase<UserEntity,UserListModel,UserDetailModel,UserEntityMapper>, IUserFacade
{
    private readonly IUserPasswordHasher _passwordHasher;

    public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper modelMapper, IUserPasswordHasher passwordHasher) : base(unitOfWorkFactory, modelMapper)
    {
        _passwordHasher = passwordHasher;
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