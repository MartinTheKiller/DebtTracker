using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.Common.Tests.Seeds;

public static class RegisteredGroupSeeds
{
    public static readonly RegisteredGroupEntity UserDebtorGroup1RegistrationEntity = new()
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

    public static readonly RegisteredGroupEntity UserCreditorGroup2RegistrationToDeleteEntity = new()
    {
        Id = Guid.Parse("e2a2b2a2-2b2a-2b2a-2b2a-2b2a2b2a2b2a"),
        UserId = UserSeeds.UserCreditorEntity.Id,
        User = UserSeeds.UserCreditorEntity,
        GroupId = GroupSeeds.Group2Entity.Id,
        Group = GroupSeeds.Group2Entity
    };

    public static readonly RegisteredGroupEntity UserToDeleteCascadeGroup1RegistrationToCascadeDeleteEntity = new()
    {
        Id = Guid.Parse("70275644-b416-4f8b-beef-415c4613deb0"),
        UserId = UserSeeds.UserToDeleteCascadeEntity.Id,
        User = UserSeeds.UserToDeleteCascadeEntity,
        GroupId = GroupSeeds.Group1Entity.Id,
        Group = GroupSeeds.Group1Entity
    };

    public static readonly RegisteredGroupEntity UserCreditorGroupToDeleteCascadeRegistrationToCascadeDeleteEntity = new()
    {
        Id = Guid.Parse("5b94e4d2-3d35-4cf7-a7df-59b1f879f19b"),
        UserId = UserSeeds.UserCreditorEntity.Id,
        User = UserSeeds.UserCreditorEntity,
        GroupId = GroupSeeds.GroupToDeleteCascadeEntity.Id,
        Group = GroupSeeds.GroupToDeleteCascadeEntity
    };

    public static readonly RegisteredGroupEntity UserDebtorGroupToDeleteCascadeRegistrationToCascadeDeleteEntity = new()
    {
        Id = Guid.Parse("584fc850-7d7b-4279-bd16-ba7379638bab"),
        UserId = UserSeeds.UserDebtorEntity.Id,
        User = UserSeeds.UserDebtorEntity,
        GroupId = GroupSeeds.GroupToDeleteCascadeEntity.Id,
        Group = GroupSeeds.GroupToDeleteCascadeEntity
    };

    public static void Seed(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<RegisteredGroupEntity>().HasData(
            UserCreditorGroup1RegistrationEntity with { Group = null, User = null },
            UserDebtorGroup1RegistrationEntity with { Group = null, User = null },
            UserCreditorGroup2RegistrationToDeleteEntity with { Group = null, User = null },
            UserToDeleteCascadeGroup1RegistrationToCascadeDeleteEntity with { Group = null, User = null },
            UserCreditorGroupToDeleteCascadeRegistrationToCascadeDeleteEntity with { Group = null, User = null }, 
            UserDebtorGroupToDeleteCascadeRegistrationToCascadeDeleteEntity with { Group = null, User = null }
        );
}