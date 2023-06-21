using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.DAL.Seeds;

public static class RegisteredGroupSeeds
{
    public static readonly RegisteredGroupEntity UserDebtorGroup1RegistrationEntity = new ()
    {
        Id = Guid.Parse("da6b22cf-8f23-4629-986a-765f22323253"),
        UserId = UserSeeds.UserDebtorEntity.Id,
        User = UserSeeds.UserDebtorEntity,
        GroupId = GroupSeeds.Group1Entity.Id,
        Group = GroupSeeds.Group1Entity
    };

    public static readonly RegisteredGroupEntity UserCreditorGroup1RegistrationEntity = new()
    {
        Id = Guid.Parse("2da25cfa-7fad-416a-94a2-72e6029e57ec"),
        UserId = UserSeeds.UserCreditorEntity.Id,
        User = UserSeeds.UserCreditorEntity,
        GroupId = GroupSeeds.Group1Entity.Id,
        Group = GroupSeeds.Group1Entity
    };

    public static void Seed(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<RegisteredGroupEntity>().HasData(
            UserCreditorGroup1RegistrationEntity with { Group = null, User = null },
            UserDebtorGroup1RegistrationEntity with { Group = null, User = null }
        );
}