using DebtTracker.DAL.Entities;

namespace DebtTracker.DAL.Mappers;

public class GroupEntityMapper : IEntityMapper<GroupEntity>
{
    public void MapToExistingEntity(GroupEntity existingEntity, GroupEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.PhotoUri = newEntity.PhotoUri;
    }
}