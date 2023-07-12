using DebtTracker.Common.Models;
using DebtTracker.DAL.Entities;

namespace DebtTracker.BL.Facades;

public interface IFacade<TEntity,TListModel,TDetailModel, in TCreateModel, in TUpdateModel>
    where TEntity : IEntity   
    where TListModel : class, IModel
    where TDetailModel : class, IModel
    where TCreateModel : class, IModel
    where TUpdateModel : class, IModel
{
    Task DeleteAsync(Guid id);
    Task<TDetailModel?> GetAsync(Guid id);
    Task<IEnumerable<TListModel>> GetAsync();
    Task<TDetailModel> CreateAsync(TCreateModel model);
    Task<TDetailModel> UpdateAsync(TUpdateModel model);
    Task<TDetailModel> SaveAsync(TCreateModel model);
}