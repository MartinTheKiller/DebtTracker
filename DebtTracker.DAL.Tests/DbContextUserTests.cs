using DebtTracker.Common.Tests;
using DebtTracker.Common.Tests.Seeds;
using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace DebtTracker.DAL.Tests;

public class DbContextUserTests : DbContextTestsBase
{
    public DbContextUserTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task NewUser_Add_Added()
    {
        // Arrange
        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = "User",
            Surname = "New",
            Email = "usernew@new.com"
        };

        // Act
        DebtTrackerDbContextSUT.Users.Add(user);
        await DebtTrackerDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users.SingleAsync(i => i.Id == user.Id);
        DeepAssert.Equal(user, actualEntity);
    }

    [Fact]
    public async Task SeededUser_GetById_Exists()
    {
        // Act
        var actualUser = await DebtTrackerDbContextSUT.Users
            .SingleOrDefaultAsync(i => i.Id == UserSeeds.UserCreditorEntity.Id);

        // Assert
        Assert.NotNull(actualUser);
        DeepAssert.Equal(UserSeeds.UserCreditorEntity, actualUser, 
            $"{nameof(UserEntity.LentDebts)}", 
            $"{nameof(UserEntity.OwesDebts)}",
            $"{nameof(UserEntity.Groups)}");
    }

    [Fact]
    public async Task SeededUser_UpdateById_Updated()
    {
        // Act
        var updatedUser = await DebtTrackerDbContextSUT.Users.SingleAsync(i => i.Id == UserSeeds.UserToUpdateEntity.Id);
        updatedUser.Name = "Updated";
        await DebtTrackerDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualUser = await dbx.Users.SingleAsync(i => i.Id == UserSeeds.UserToUpdateEntity.Id);
        Assert.Equal(updatedUser.Name, actualUser.Name);
    }

    [Fact]
    public async Task SeededUser_DeleteById_Deleted()
    {
        // Act
        var userToDelete = await DebtTrackerDbContextSUT.Users.SingleAsync(i => i.Id == UserSeeds.UserToDeleteEntity.Id);
        DebtTrackerDbContextSUT.Users.Remove(userToDelete);
        await DebtTrackerDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualUser = await dbx.Users.SingleOrDefaultAsync(i => i.Id == UserSeeds.UserToDeleteEntity.Id);
        Assert.Null(actualUser);
    }
}