using DebtTracker.BL.Models;
using DebtTracker.DAL.Entities;

namespace DebtTracker.BL.Facades;

public interface IFacade<TEntity,TListModel,TDetailModel>
    where TEntity : IEntity   
    where TListModel : class, IModel
    where TDetailModel : class, IModel
{
    Task DeleteAsync(Guid id);
    Task<TDetailModel?> GetAsync(Guid id);
    Task<IEnumerable<TListModel>> GetAsync();
    Task<TDetailModel> SaveAsync(TDetailModel model);
}