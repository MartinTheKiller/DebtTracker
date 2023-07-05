using DebtTracker.BL.Models;
using DebtTracker.BL.Models.Group;
using DebtTracker.DAL.Entities;

namespace DebtTracker.BL.Facades;

public interface IGroupFacade : IFacade<GroupEntity, GroupListModel, GroupDetailModel, GroupDetailModel, GroupDetailModel>
{
    
}