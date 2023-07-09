using DebtTracker.BL.Models;
using DebtTracker.BL.Models.Debt;
using DebtTracker.DAL.Entities;

namespace DebtTracker.BL.Facades;

public interface IDebtFacade : IFacade<DebtEntity, DebtListModel, DebtDetailModel, DebtDetailModel, DebtDetailModel>
{

}