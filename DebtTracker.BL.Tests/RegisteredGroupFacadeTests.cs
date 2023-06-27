using DebtTracker.BL.Facades;
using DebtTracker.BL.Models;
using DebtTracker.Common.Tests;
using DebtTracker.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace DebtTracker.BL.Tests;

public class RegisteredGroupFacadeTests : FacadeTestsBase
{
    private readonly RegisteredGroupFacade FacadeSUT;

    public RegisteredGroupFacadeTests(ITestOutputHelper output) : base(output)
    {
        FacadeSUT = new RegisteredGroupFacade(UnitOfWorkFactory, Mapper);
    }

    [Fact]
    public async Task NewRegisteredGroup_EmptyId_Save_Added()
    {
        // Arrange
        var registeredGroup = new RegisteredGroupModel
        {
            Id = Guid.Empty,
            UserId = UserSeeds.UserToRegisterEntity.Id,
            GroupId = GroupSeeds.GroupToRegisterEntity.Id,
        };

        // Act
        var savedRegisteredGroup = await FacadeSUT.SaveAsync(registeredGroup);

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.RegisteredGroups.SingleAsync(i => i.Id == savedRegisteredGroup.Id);
        DeepAssert.Equal(registeredGroup with { Id = savedRegisteredGroup.Id }, Mapper.Map<RegisteredGroupModel>(actualEntity));

        // Clean up
        dbx.RegisteredGroups.Remove(actualEntity);
    }

    [Fact]
    public async Task NewRegisteredGroup_NewId_Save_Added()
    {
        // Arrange
        var registeredGroup = new RegisteredGroupModel
        {
            Id = Guid.NewGuid(),
            UserId = UserSeeds.UserToRegisterEntity.Id,
            GroupId = GroupSeeds.GroupToRegisterEntity.Id,
        };

        // Act
        var savedRegisteredGroup = await FacadeSUT.SaveAsync(registeredGroup);

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.RegisteredGroups.SingleAsync(i => i.Id == savedRegisteredGroup.Id);
        DeepAssert.Equal(registeredGroup with { Id = savedRegisteredGroup.Id }, Mapper.Map<RegisteredGroupModel>(actualEntity));
    }

    [Fact]
    public async Task SeededRegisteredGroup_GetById_Exists()
    {
        // Act
        var actualRegisteredGroup = await FacadeSUT.GetAsync(RegisteredGroupSeeds.UserDebtorGroup1RegistrationEntity.Id);

        // Assert
        Assert.NotNull(actualRegisteredGroup);
        DeepAssert.Equal(Mapper.Map<RegisteredGroupModel>(RegisteredGroupSeeds.UserDebtorGroup1RegistrationEntity), actualRegisteredGroup);
    }

    [Fact]
    public async Task SeededRegisteredGroup_UpdateById_Throws()
    {
        // Assert
        var updatedRegisteredGroup = await FacadeSUT.GetAsync(RegisteredGroupSeeds.UserDebtorGroup1RegistrationEntity.Id);
        updatedRegisteredGroup.UserId = UserSeeds.UserToRegisterEntity.Id;
        await Assert.ThrowsAnyAsync<InvalidOperationException>(async () => await FacadeSUT.SaveAsync(updatedRegisteredGroup));
    }

    [Fact]
    public async Task SeededRegisteredGroup_DeleteById_Deleted()
    {
        // Act
        await FacadeSUT.DeleteAsync(RegisteredGroupSeeds.UserCreditorGroup2RegistrationToDeleteEntity.Id);

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualRegisteredGroup = await dbx.RegisteredGroups.SingleOrDefaultAsync(i => i.Id == RegisteredGroupSeeds.UserCreditorGroup2RegistrationToDeleteEntity.Id);
        Assert.Null(actualRegisteredGroup);
    }
}