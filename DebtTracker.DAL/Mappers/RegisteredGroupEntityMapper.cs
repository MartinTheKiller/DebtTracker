using DebtTracker.DAL.Entities;

namespace DebtTracker.DAL.Mappers;

public class RegisteredGroupEntityMapper : IEntityMapper<RegisteredGroupEntity>
{
    public void MapToExistingEntity(RegisteredGroupEntity existingEntity, RegisteredGroupEntity newEntity)
    {
        existingEntity.UserId = newEntity.UserId;
        existingEntity.GroupId = newEntity.GroupId;
    }
}