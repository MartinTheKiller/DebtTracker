using DebtTracker.BL.Hashers;
using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.Common.Tests.Seeds;

public static class UserSeeds
{
    public const string DefaultPassword = "Password123";
    private static readonly UserPasswordHasher _passwordHasher = new();

    public static readonly UserEntity UserDebtorEntity = new()
    {
        Id = Guid.Parse("1a2b20f1-a388-4f7e-899d-1bb812cf96e0"),
        Name = "Marten",
        Surname = "NoMoney",
        Email = "MartenNoMoney@email.com",
        BankAccount = "123456/1234",
        PhotoUri = null,
        HashedPassword = _passwordHasher.HashPassword(DefaultPassword)
    };

    public static readonly UserEntity UserCreditorEntity = new()
    {
        Id = Guid.Parse("7fa0a0b3-4f8e-4650-8d50-f1a3634bf153"),
        Name = "Marten",
        Surname = "HasMoney",
        Email = "MartenHasMoney@email.com",
        BankAccount = "654321/4321",
        PhotoUri = null,
        HashedPassword = _passwordHasher.HashPassword(DefaultPassword)
    };

    public static readonly UserEntity UserToUpdateEntity = UserDebtorEntity with { Id = Guid.Parse("6f11c8ab-e99d-4abd-87d4-ecf26874d11d"), 
                                                                                    Groups = new List<RegisteredGroupEntity>(), 
                                                                                    LentDebts = new List<DebtEntity>(), 
                                                                                    OwesDebts = new List<DebtEntity>()};
    public static readonly UserEntity UserToDeleteEntity = UserDebtorEntity with { Id = Guid.Parse("3aa80c18-79ee-48bd-9bff-e864dfed62a5"), 
                                                                                    Groups = new List<RegisteredGroupEntity>(), 
                                                                                    LentDebts = new List<DebtEntity>(), 
                                                                                    OwesDebts = new List<DebtEntity>()};
    public static readonly UserEntity UserToRegisterEntity = UserDebtorEntity with { Id = Guid.Parse("2ff889ff-4aae-46a9-b571-440ecf71c5cf"), 
                                                                                     Groups = new List<RegisteredGroupEntity>(), 
                                                                                     LentDebts = new List<DebtEntity>(), 
                                                                                     OwesDebts = new List<DebtEntity>()};
    public static readonly UserEntity UserToDeleteCascadeEntity = UserDebtorEntity with { Id = Guid.Parse("7c18fbeb-4a6c-450e-94cb-c87f645846f2"), 
                                                                                           Groups = new List<RegisteredGroupEntity>(), 
                                                                                           LentDebts = new List<DebtEntity>(), 
                                                                                           OwesDebts = new List<DebtEntity>()};
    public static readonly UserEntity UserToChangePasswordEntity = UserDebtorEntity with { Id = Guid.Parse("c3361cda-d21a-42d6-9b6f-a4d275be5eb8"),
                                                                                            Groups = new List<RegisteredGroupEntity>(),
                                                                                            LentDebts = new List<DebtEntity>(),
                                                                                            OwesDebts = new List<DebtEntity>()};

    public static void Seed(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<UserEntity>().HasData(
            UserDebtorEntity with { Groups = new List<RegisteredGroupEntity>(), LentDebts = new List<DebtEntity>(), OwesDebts = new List<DebtEntity>() },
            UserCreditorEntity with { Groups = new List<RegisteredGroupEntity>(), LentDebts = new List<DebtEntity>(), OwesDebts = new List<DebtEntity>() },
            UserToUpdateEntity with { Groups = new List<RegisteredGroupEntity>(), LentDebts = new List<DebtEntity>(), OwesDebts = new List<DebtEntity>() },
            UserToDeleteEntity with { Groups = new List<RegisteredGroupEntity>(), LentDebts = new List<DebtEntity>(), OwesDebts = new List<DebtEntity>() },
            UserToRegisterEntity with { Groups = new List<RegisteredGroupEntity>(), LentDebts = new List<DebtEntity>(), OwesDebts = new List<DebtEntity>() },
            UserToDeleteCascadeEntity with { Groups = new List<RegisteredGroupEntity>(), LentDebts = new List<DebtEntity>(), OwesDebts = new List<DebtEntity>() },
            UserToChangePasswordEntity with { Groups = new List<RegisteredGroupEntity>(), LentDebts = new List<DebtEntity>(), OwesDebts = new List<DebtEntity>() }
        );
}