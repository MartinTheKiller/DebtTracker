using DebtTracker.Common.Models;
using DebtTracker.DAL.Entities;

namespace DebtTracker.BL.Facades;

public interface IGroupFacade : IFacade<GroupEntity, GroupListModel, GroupDetailModel, GroupDetailModel, GroupDetailModel>
{
    
}