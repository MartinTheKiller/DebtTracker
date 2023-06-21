using DebtTracker.DAL.Entities;

namespace DebtTracker.DAL.Mappers;

public class UserEntityMapper : IEntityMapper<UserEntity>
{
    public void MapToExistingEntity(UserEntity existingEntity, UserEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.Surname = newEntity.Surname;
        existingEntity.BankAccount = newEntity.BankAccount;
        existingEntity.Email = newEntity.Email;
        existingEntity.PhotoUri = newEntity.PhotoUri;
    }
}