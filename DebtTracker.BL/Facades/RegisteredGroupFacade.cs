using AutoMapper;
using CubiTracker.DAL.Repositories;
using DebtTracker.BL.Models;
using DebtTracker.DAL.Entities;
using DebtTracker.DAL.Mappers;
using DebtTracker.DAL.UnitOfWork;

namespace DebtTracker.BL.Facades;

public class RegisteredGroupFacade : FacadeBase<RegisteredGroupEntity, RegisteredGroupModel, RegisteredGroupModel, RegisteredGroupEntityMapper>, IRegisteredGroupFacade
{
    public RegisteredGroupFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper modelMapper) : base(unitOfWorkFactory, modelMapper)
    {
    }

    public new async Task<RegisteredGroupModel> SaveAsync(RegisteredGroupModel model)
    {
        var uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<RegisteredGroupEntity, RegisteredGroupEntityMapper>();

        if (await repository.ExistsAsync(ModelMapper.Map<RegisteredGroupEntity>(model)))
            throw new InvalidOperationException("Group registrations are not meant to be updated");

        return await base.SaveAsync(model);
    }
}