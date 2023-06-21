using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.Common.Tests.Seeds;

public static class GroupSeeds
{
    public static readonly GroupEntity Group1Entity = new()
    {
        Id = Guid.Parse("480906a5-b7f5-4000-95bf-a87b4675c5e2"),
        Name = "Group 1"
    };

    public static readonly GroupEntity Group2Entity = new()
    {
        Id = Guid.Parse("bba2ad60-c789-4eca-af1e-3e0fbbe528c7"),
        Name = "Group 2"
    };

    public static readonly GroupEntity GroupToUpdateEntity = Group1Entity with { Id = Guid.Parse("160811b4-6ba4-48ab-b4c2-a4eb42f5df42"),
                                                                                    Debts = new List<DebtEntity>(),
                                                                                    Users = new List<RegisteredGroupEntity>()};
    public static readonly GroupEntity GroupToDeleteEntity = Group1Entity with { Id = Guid.Parse("1afb424b-4e69-45dc-b49b-4fd700f4ef87"),
                                                                                    Debts = new List<DebtEntity>(),
                                                                                    Users = new List<RegisteredGroupEntity>()};

    public static readonly GroupEntity GroupToRegisterEntity = Group1Entity with { Id = Guid.Parse("da8b0ee9-075b-47cd-a302-938db4bd2d46"),
                                                                                    Debts = new List<DebtEntity>(),
                                                                                    Users = new List<RegisteredGroupEntity>()};

    public static readonly GroupEntity GroupToDeleteCascadeEntity = Group1Entity with { Id = Guid.Parse("d515c631-03b2-4eb9-aa80-e33579b17ab8"),
                                                                                        Debts = new List<DebtEntity>(),
                                                                                        Users = new List<RegisteredGroupEntity>()};

    public static void Seed(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<GroupEntity>().HasData(
            Group1Entity with { Users = new List<RegisteredGroupEntity>(), Debts = new List<DebtEntity>() },
            Group2Entity with { Users = new List<RegisteredGroupEntity>(), Debts = new List<DebtEntity>() },
            GroupToUpdateEntity with { Users = new List<RegisteredGroupEntity>(), Debts = new List<DebtEntity>() },
            GroupToDeleteEntity with { Users = new List<RegisteredGroupEntity>(), Debts = new List<DebtEntity>() },
            GroupToRegisterEntity with { Users = new List<RegisteredGroupEntity>(), Debts = new List<DebtEntity>() },
            GroupToDeleteCascadeEntity with { Users = new List<RegisteredGroupEntity>(), Debts = new List<DebtEntity>() }
        );
}