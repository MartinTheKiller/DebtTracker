using DebtTracker.BL.Models;
using DebtTracker.BL.Models.User;
using DebtTracker.DAL.Entities;

namespace DebtTracker.BL.Facades;

public interface IUserFacade : IFacade<UserEntity, UserListModel, UserDetailModel, UserCreateModel, UserDetailModel>
{
    Task<Guid?> LoginAsync(string email, string password);
}