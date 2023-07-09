using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.DAL.Seeds;

public static class UserSeeds
{
    public static readonly UserEntity UserDebtorEntity = new()
    {
        Id = Guid.Parse("1a2b20f1-a388-4f7e-899d-1bb812cf96e0"),
        Name = "Marten",
        Surname = "NoMoney",
        BankAccount = "123456/1234",
        Email = "MartenNoMoney@email.com",
        PhotoUri = null,
        HashedPassword = "$2a$10$CMRZOYCWjOa8qztdPYY3S.wsBZmLIIWyf3KQVbeQ3ccbBo9gqcRDG" //Password123
    };

    public static readonly UserEntity UserCreditorEntity = new()
    {
        Id = Guid.Parse("7fa0a0b3-4f8e-4650-8d50-f1a3634bf153"),
        Name = "Marten",
        Surname = "HasMoney",
        BankAccount = "654321/4321",
        Email = "MartenHasMoney@email.com",
        PhotoUri = null,
        HashedPassword = "$2a$10$CMRZOYCWjOa8qztdPYY3S.wsBZmLIIWyf3KQVbeQ3ccbBo9gqcRDG" //Password123
    };

    public static void Seed(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<UserEntity>().HasData(
            UserDebtorEntity with { Groups = new List<RegisteredGroupEntity>(), LentDebts = new List<DebtEntity>(), OwesDebts = new List<DebtEntity>() },
            UserCreditorEntity with { Groups = new List<RegisteredGroupEntity>(), LentDebts = new List<DebtEntity>(), OwesDebts = new List<DebtEntity>() }
        );
}