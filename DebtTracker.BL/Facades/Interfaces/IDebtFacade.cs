using DebtTracker.Common.Models;
using DebtTracker.DAL.Entities;

namespace DebtTracker.BL.Facades;

public interface IDebtFacade : IFacade<DebtEntity, DebtListModel, DebtDetailModel, DebtDetailModel, DebtDetailModel>
{

}