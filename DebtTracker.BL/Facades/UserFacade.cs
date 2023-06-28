using AutoMapper;
using DebtTracker.BL.Models;
using DebtTracker.DAL.Entities;
using DebtTracker.DAL.Mappers;
using DebtTracker.DAL.UnitOfWork;

namespace DebtTracker.BL.Facades;

public class UserFacade : FacadeBase<UserEntity,UserListModel,UserDetailModel,UserEntityMapper>, IUserFacade
{
    public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper modelMapper) : base(unitOfWorkFactory, modelMapper)
    {
    }
}