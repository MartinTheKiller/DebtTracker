using AutoMapper;
using DebtTracker.BL.Models;
using DebtTracker.BL.Models.Debt;
using DebtTracker.DAL.Entities;
using DebtTracker.DAL.Mappers;
using DebtTracker.DAL.UnitOfWork;

namespace DebtTracker.BL.Facades;

public class DebtFacade : FacadeBase<DebtEntity, DebtListModel, DebtDetailModel, DebtDetailModel, DebtDetailModel, DebtEntityMapper>, IDebtFacade
{
    public DebtFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper modelMapper) : base(unitOfWorkFactory, modelMapper)
    {
    }
}