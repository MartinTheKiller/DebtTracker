using DebtTracker.BL.Models;
using DebtTracker.DAL.Entities;

namespace DebtTracker.BL.Facades;

public interface IDebtFacade : IFacade<DebtEntity, DebtListModel, DebtDetailModel>
{
    
}