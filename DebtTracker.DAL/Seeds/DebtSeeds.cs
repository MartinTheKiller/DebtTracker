using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.DAL.Seeds;

public static class DebtSeeds
{
    public static readonly DebtEntity UserCreditorUserDebtorDebt1Entity = new()
    {
        Id = Guid.Parse("52dfcff9-e31c-49cc-bd65-c6289a58b7bf"),
        Name = "Coffee",
        Amount = 50,
        Date = DateTime.Parse("1/2/2023"),
        Description = "You didn't have money, lol",
        ResolvedDate = null,
        DebtorId = UserSeeds.UserDebtorEntity.Id,
        Debtor = UserSeeds.UserDebtorEntity,
        CreditorId = UserSeeds.UserCreditorEntity.Id,
        Creditor = UserSeeds.UserCreditorEntity,
        GroupId = GroupSeeds.Group1Entity.Id,
        Group = GroupSeeds.Group1Entity
    };

    public static readonly DebtEntity UserCreditorUserDebtorDebt2Entity = new()
    {
        Id = Guid.Parse("a5ed99cb-08ee-48cf-b7fb-72f86ddbe7f3"),
        Name = "Bus",
        Amount = 12,
        Date = DateTime.Parse("2/2/2023"),
        Description = "You didn't have money again, lol",
        ResolvedDate = null,
        DebtorId = UserSeeds.UserDebtorEntity.Id,
        Debtor = UserSeeds.UserDebtorEntity,
        CreditorId = UserSeeds.UserCreditorEntity.Id,
        Creditor = UserSeeds.UserCreditorEntity,
        GroupId = GroupSeeds.Group1Entity.Id,
        Group = GroupSeeds.Group1Entity
    };

    public static void Seed(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<DebtEntity>().HasData(
            UserCreditorUserDebtorDebt1Entity with { Creditor = null, Debtor = null, Group = null },
            UserCreditorUserDebtorDebt2Entity with { Creditor = null, Debtor = null, Group = null }
        );
}