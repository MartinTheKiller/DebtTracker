using DebtTracker.BL.Models;
using DebtTracker.DAL.Entities;

namespace DebtTracker.BL.Facades;

public interface IUserFacade : IFacade<UserEntity, UserListModel, UserDetailModel>
{
    
}