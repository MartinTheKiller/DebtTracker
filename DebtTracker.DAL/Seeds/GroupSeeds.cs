using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.DAL.Seeds;

public static class GroupSeeds
{
    public static readonly GroupEntity Group1Entity = new()
    {
        Id = Guid.Parse("480906a5-b7f5-4000-95bf-a87b4675c5e2"),
        Name = "Group 1"
    };

    static GroupSeeds()
    {
        Group1Entity.Users.Add(RegisteredGroupSeeds.UserCreditorGroup1RegistrationEntity);
        Group1Entity.Users.Add(RegisteredGroupSeeds.UserDebtorGroup1RegistrationEntity);
    }

    public static void Seed(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<GroupEntity>().HasData(
            Group1Entity with { Users = new List<RegisteredGroupEntity>(), Debts = new List<DebtEntity>() }
        );
}