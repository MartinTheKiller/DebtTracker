using DebtTracker.Common.Tests;
using DebtTracker.Common.Tests.Seeds;
using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace DebtTracker.DAL.Tests;

public class DbContextDebtTests : DbContextTestsBase
{
    public DbContextDebtTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task NewDebt_Add_Added()
    {
        // Arrange
        var debt = new DebtEntity
        {
            Id = Guid.Parse("c77077c8-9a3d-496a-b423-c45b0e887ca5"),
            Name = "New debt",
            Amount = 100,
            Date = DateTime.Now,
            CreditorId = UserSeeds.UserCreditorEntity.Id,
            DebtorId = UserSeeds.UserDebtorEntity.Id,
            GroupId = GroupSeeds.Group1Entity.Id
        };

        // Act
        DebtTrackerDbContextSUT.Debts.Add(debt);
        await DebtTrackerDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualDebt = await dbx.Debts.SingleAsync(i => i.Id == debt.Id);
        DeepAssert.Equal(debt, actualDebt, 
            $"{nameof(DebtEntity.Date)}", 
            $"{nameof(DebtEntity.Creditor)}",
            $"{nameof(DebtEntity.Debtor)}",
            $"{nameof(DebtEntity.Group)}");
        Assert.Equal(0, DateTime.Compare(actualDebt.Date, debt.Date));
    }

    [Fact]
    public async Task SeededDebt_GetById_Exists()
    {
        // Act
        var actualDebt = await DebtTrackerDbContextSUT.Debts
            .SingleOrDefaultAsync(i => i.Id == DebtSeeds.UserCreditorUserDebtorDebt1Entity.Id);

        // Assert
        Assert.NotNull(actualDebt);
        DeepAssert.Equal(DebtSeeds.UserCreditorUserDebtorDebt1Entity, actualDebt, 
            $"{nameof(DebtEntity.Creditor)}",
            $"{nameof(DebtEntity.Debtor)}",
            $"{nameof(DebtEntity.Group)}");
    }

    [Fact]
    public async Task SeededDebt_UpdateById_Updated()
    {
        // Act
        var updatedDebt = await DebtTrackerDbContextSUT.Debts.SingleAsync(i => i.Id == DebtSeeds.UserCreditorUserDebtorDebtToUpdateEntity.Id);
        updatedDebt.Amount = 123;
        await DebtTrackerDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualDebt = await dbx.Debts.SingleAsync(i => i.Id == DebtSeeds.UserCreditorUserDebtorDebtToUpdateEntity.Id);
        DeepAssert.Equal(updatedDebt, actualDebt, 
            $"{nameof(DebtEntity.Creditor)}",
            $"{nameof(DebtEntity.Debtor)}",
            $"{nameof(DebtEntity.Group)}");
    }

    [Fact]
    public async Task SeededDebt_DeleteById_Deleted()
    {
        // Act
        var debtToDelete = await DebtTrackerDbContextSUT.Debts.SingleAsync(i => i.Id == DebtSeeds.UserCreditorUserDebtorDebtToDeleteEntity.Id);
        DebtTrackerDbContextSUT.Debts.Remove(debtToDelete);
        await DebtTrackerDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualDebt = await dbx.Debts.SingleOrDefaultAsync(i => i.Id == DebtSeeds.UserCreditorUserDebtorDebtToDeleteEntity.Id);
        Assert.Null(actualDebt);
    }
}