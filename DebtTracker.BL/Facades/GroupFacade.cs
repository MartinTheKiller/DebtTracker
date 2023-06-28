using AutoMapper;
using DebtTracker.BL.Models;
using DebtTracker.DAL.Entities;
using DebtTracker.DAL.Mappers;
using DebtTracker.DAL.UnitOfWork;

namespace DebtTracker.BL.Facades;

public class GroupFacade : FacadeBase<GroupEntity, GroupListModel, GroupDetailModel, GroupEntityMapper>, IGroupFacade
{
    public GroupFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper modelMapper) : base(unitOfWorkFactory, modelMapper)
    {
    }
}