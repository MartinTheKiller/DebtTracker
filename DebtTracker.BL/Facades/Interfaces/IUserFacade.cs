using DebtTracker.Common.Models;
using DebtTracker.DAL.Entities;

namespace DebtTracker.BL.Facades;

public interface IUserFacade : IFacade<UserEntity, UserListModel, UserDetailModel, UserCreateModel, UserDetailModel>
{
    Task<Guid?> LoginAsync(string email, string password);
    Task<bool> ChangePasswordAsync(Guid id, string oldPassword, string newPassword);
}