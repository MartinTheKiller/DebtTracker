using DebtTracker.Common.Tests;
using DebtTracker.Common.Tests.Seeds;
using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace DebtTracker.DAL.Tests;

public class DbContextRegisteredGroupTests : DbContextTestsBase
{
    public DbContextRegisteredGroupTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task NewRegistration_Add_Added()
    {
        // Arrange
        var registration = new RegisteredGroupEntity
        {
            Id = Guid.Parse("4643aa09-918d-401b-8438-a9a3bbf9c1f3"),
            UserId = UserSeeds.UserToRegisterEntity.Id,
            GroupId = GroupSeeds.GroupToRegisterEntity.Id
        };

        // Act
        DebtTrackerDbContextSUT.RegisteredGroups.Add(registration);
        await DebtTrackerDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualRegistration = await dbx.RegisteredGroups.SingleAsync(i => i.Id == registration.Id);
        DeepAssert.Equal(registration, actualRegistration,
                                $"{nameof(RegisteredGroupEntity.Group)}",
                                $"{nameof(RegisteredGroupEntity.User)}");
    }

    [Fact]
    public async Task SeededRegistration_GetById_Exists()
    {
        // Act
        var actualRegistration = await DebtTrackerDbContextSUT.RegisteredGroups
            .SingleOrDefaultAsync(i => i.Id == RegisteredGroupSeeds.UserDebtorGroup1RegistrationEntity.Id);

        // Assert
        Assert.NotNull(actualRegistration);
        DeepAssert.Equal(RegisteredGroupSeeds.UserDebtorGroup1RegistrationEntity, actualRegistration,
                       $"{nameof(RegisteredGroupEntity.Group)}",
                                  $"{nameof(RegisteredGroupEntity.User)}");
    }

    [Fact]
    public async Task SeededRegistration_DeleteById_Deleted()
    {
        // Act
        var registrationToDelete = await DebtTrackerDbContextSUT.RegisteredGroups
            .SingleAsync(i => i.Id == RegisteredGroupSeeds.UserCreditorGroup2RegistrationToDeleteEntity.Id);
        DebtTrackerDbContextSUT.RegisteredGroups.Remove(registrationToDelete);
        await DebtTrackerDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualRegistration = await dbx.RegisteredGroups.SingleOrDefaultAsync(i => i.Id == RegisteredGroupSeeds.UserCreditorGroup2RegistrationToDeleteEntity.Id);
        Assert.Null(actualRegistration);
    }

    [Fact]
    public async Task NewRegistrationWithNotExistingUser_Add_Throws()
    {
        // Arrange
        var registration = new RegisteredGroupEntity
        {
            Id = Guid.Parse("4643aa09-918d-401b-8438-a9a3bbf9c1f3"),
            GroupId = GroupSeeds.GroupToRegisterEntity.Id,
            UserId = Guid.Parse("c2b27c1b-8a57-4821-9dd6-3e7e07621d64")
        };

        // Act
        DebtTrackerDbContextSUT.RegisteredGroups.Add(registration);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => DebtTrackerDbContextSUT.SaveChangesAsync());
    }

    [Fact]
    public async Task NewRegistrationWithNotExistingGroup_Add_Throws()
    {
        // Arrange
        var registration = new RegisteredGroupEntity
        {
            Id = Guid.Parse("4643aa09-918d-401b-8438-a9a3bbf9c1f3"),
            GroupId = Guid.Parse("c2b27c1b-8a57-4821-9dd6-3e7e07621d64"),
            UserId = UserSeeds.UserToRegisterEntity.Id
        };

        // Act
        DebtTrackerDbContextSUT.RegisteredGroups.Add(registration);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => DebtTrackerDbContextSUT.SaveChangesAsync());
    }
}