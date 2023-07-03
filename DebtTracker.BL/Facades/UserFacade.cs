using AutoMapper;
using DebtTracker.BL.Models;
using DebtTracker.DAL.Entities;
using DebtTracker.DAL.Mappers;
using DebtTracker.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.BL.Facades;

public class UserFacade : FacadeBase<UserEntity,UserListModel,UserDetailModel,UserEntityMapper>, IUserFacade
{
    public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper modelMapper) : base(unitOfWorkFactory, modelMapper)
    {
    }

    public async Task<UserLoginModel?> GetAsync(UserLoginModel model)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<UserEntity> query = uow
            .GetRepository<UserEntity, UserEntityMapper>()
            .Get()
            .Where(e => e.Email == model.Email);
        return await ModelMapper.ProjectTo<UserLoginModel>(query).SingleOrDefaultAsync().ConfigureAwait(false);
    }
}