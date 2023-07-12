using DebtTracker.BL.Facades;
using DebtTracker.Common.Models;
using DebtTracker.Common.Tests;
using DebtTracker.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace DebtTracker.BL.Tests;

public class DebtFacadeTests : FacadeTestsBase
{
    private readonly DebtFacade FacadeSUT;

    public DebtFacadeTests(ITestOutputHelper output) : base(output)
    {
        FacadeSUT = new DebtFacade(UnitOfWorkFactory, Mapper);
    }

    [Fact]
    public async Task NewDebt_EmptyId_Save_Added()
    {
        // Arrange
        var debt = new DebtDetailModel
        {
            Id = Guid.Empty,
            Name = "New debt",
            Amount = 50,
            Date = DateTime.Now,
            DebtorId = UserSeeds.UserCreditorEntity.Id,
            CreditorId = UserSeeds.UserCreditorEntity.Id,
            GroupId = GroupSeeds.Group1Entity.Id
        };

        // Act
        var savedDebt = await FacadeSUT.SaveAsync(debt);

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Debts.SingleAsync(i => i.Id == savedDebt.Id);
        DeepAssert.Equal(debt with { Id = savedDebt.Id }, Mapper.Map<DebtDetailModel>(actualEntity));
    }

    [Fact]
    public async Task NewDebt_NewId_Save_Added()
    {
        // Arrange
        var debt = new DebtDetailModel
        {
            Id = Guid.NewGuid(),
            Name = "New debt",
            Amount = 50,
            Date = DateTime.Today,
            DebtorId = UserSeeds.UserCreditorEntity.Id,
            CreditorId = UserSeeds.UserCreditorEntity.Id,
            GroupId = GroupSeeds.Group1Entity.Id
        };

        // Act
        var savedDebt = await FacadeSUT.SaveAsync(debt);

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Debts.SingleAsync(i => i.Id == savedDebt.Id);
        DeepAssert.Equal(debt with { Id = savedDebt.Id }, Mapper.Map<DebtDetailModel>(actualEntity));
    }

    [Fact]
    public async Task SeededDebt_GetById_Exists()
    {
        // Act
        var actualDebt = await FacadeSUT.GetAsync(DebtSeeds.UserCreditorUserDebtorDebt1Entity.Id);

        // Assert
        Assert.NotNull(actualDebt);
        DeepAssert.Equal(Mapper.Map<DebtDetailModel>(DebtSeeds.UserCreditorUserDebtorDebt1Entity), actualDebt);
    }

    [Fact]
    public async Task SeededDebt_UpdateById_Updated()
    {
        // Act
        var updatedDebt = await FacadeSUT.GetAsync(DebtSeeds.UserCreditorUserDebtorDebtToUpdateEntity.Id);
        updatedDebt.Name = "Updated";
        await FacadeSUT.SaveAsync(updatedDebt);
        DebtSeeds.UserCreditorUserDebtorDebtToUpdateEntity.Name = "Updated";

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualDebt = await dbx.Debts.SingleAsync(i => i.Id == DebtSeeds.UserCreditorUserDebtorDebtToUpdateEntity.Id);
        DeepAssert.Equal(updatedDebt, Mapper.Map<DebtDetailModel>(actualDebt));
    }

    [Fact]
    public async Task SeededDebt_DeleteById_Deleted()
    {
        // Act
        await FacadeSUT.DeleteAsync(DebtSeeds.UserCreditorUserDebtorDebtToDeleteEntity.Id);

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualDebt = await dbx.Debts.SingleOrDefaultAsync(i => i.Id == DebtSeeds.UserCreditorUserDebtorDebtToDeleteEntity.Id);
        Assert.Null(actualDebt);
    }
}