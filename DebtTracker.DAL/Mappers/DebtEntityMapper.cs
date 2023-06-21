using DebtTracker.DAL.Entities;

namespace DebtTracker.DAL.Mappers;

public class DebtEntityMapper : IEntityMapper<DebtEntity>
{
    public void MapToExistingEntity(DebtEntity existingEntity, DebtEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.Amount = newEntity.Amount;
        existingEntity.Date = newEntity.Date;
        existingEntity.Description = newEntity.Description;
        existingEntity.ResolvedDate = newEntity.ResolvedDate;
        existingEntity.CreditorId = newEntity.CreditorId;
        existingEntity.DebtorId = newEntity.DebtorId;
        existingEntity.GroupId = newEntity.GroupId;
    }
}