using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.Common.Tests.Seeds;

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

    public static readonly DebtEntity UserCreditorUserDebtorDebtToUpdateEntity = UserCreditorUserDebtorDebt1Entity with { Id = Guid.Parse("69bd93e6-8fa1-49d5-8fc8-2261401e3b79") };
    public static readonly DebtEntity UserCreditorUserDebtorDebtToDeleteEntity = UserCreditorUserDebtorDebt1Entity with { Id = Guid.Parse("c08a164a-8091-4172-a356-34167777fb20") };
    
    public static readonly DebtEntity UserToDeleteCascadeUserDebtorDebtToCascadeDeleteEntity = UserCreditorUserDebtorDebt1Entity with { Id = Guid.Parse("3485dbb1-406c-4b4e-ba45-65fabe47a9e0"), 
                                                                                                                                        CreditorId = UserSeeds.UserToDeleteCascadeEntity.Id, 
                                                                                                                                        Creditor = UserSeeds.UserToDeleteCascadeEntity};
    public static readonly DebtEntity UserCreditorUserToDeleteCascadeDebtToCascadeDeleteEntity = UserCreditorUserDebtorDebt1Entity with { Id = Guid.Parse("977303a7-eb4d-47c8-a8a6-4a656744a42a"),
                                                                                                                                            DebtorId = UserSeeds.UserToDeleteCascadeEntity.Id,
                                                                                                                                            Debtor = UserSeeds.UserToDeleteCascadeEntity};

    public static readonly DebtEntity UserCreditorUserDebtorDebtToCascadeDeleteEntity = UserCreditorUserDebtorDebt1Entity with { Id = Guid.Parse("ca04936c-8133-4ef6-bf60-f1bb7e91aabd"),
                                                                                                                                    GroupId = GroupSeeds.GroupToDeleteCascadeEntity.Id,
                                                                                                                                    Group = GroupSeeds.GroupToDeleteCascadeEntity};

    public static void Seed(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<DebtEntity>().HasData(
            UserCreditorUserDebtorDebt1Entity with { Creditor = null, Debtor = null, Group = null },
            UserCreditorUserDebtorDebt2Entity with { Creditor = null, Debtor = null, Group = null },
            UserCreditorUserDebtorDebtToUpdateEntity with { Creditor = null, Debtor = null, Group = null },
            UserCreditorUserDebtorDebtToDeleteEntity with { Creditor = null, Debtor = null, Group = null },
            UserToDeleteCascadeUserDebtorDebtToCascadeDeleteEntity with { Creditor = null, Debtor = null, Group = null },
            UserCreditorUserToDeleteCascadeDebtToCascadeDeleteEntity with { Creditor = null, Debtor = null, Group = null },
            UserCreditorUserDebtorDebtToCascadeDeleteEntity with { Creditor = null, Debtor = null, Group = null }
        );
}