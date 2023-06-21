using DebtTracker.Common.Tests.Seeds;
using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace DebtTracker.DAL.Tests;

public class DbContextConstraintsTests : DbContextTestsBase
{
    public DbContextConstraintsTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task SeededUser_DeleteById_DebtsDeleted_GroupRegistrationsDeleted()
    {

        // Act
        var userToDelete = await DebtTrackerDbContextSUT.Users
            .SingleAsync(i => i.Id == UserSeeds.UserToDeleteCascadeEntity.Id);
        DebtTrackerDbContextSUT.Users.Remove(userToDelete);
        await DebtTrackerDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualUser = await dbx.Users.SingleOrDefaultAsync(i => i.Id == UserSeeds.UserToDeleteCascadeEntity.Id);
        Assert.Null(actualUser);
        
        var actualDebts = await dbx.Debts.Where(i => i.CreditorId == UserSeeds.UserToDeleteCascadeEntity.Id 
                                                                            || i.DebtorId == UserSeeds.UserToDeleteCascadeEntity.Id).ToListAsync();
        Assert.Empty(actualDebts);
        
        var actualRegistrations = await dbx.RegisteredGroups.Where(i => i.UserId == UserSeeds.UserToDeleteCascadeEntity.Id).ToListAsync();
        Assert.Empty(actualRegistrations);
    }

    [Fact]
    public async Task SeededGroup_DeleteById_DebtsDeleted_GroupRegistrationsDeleted()
    {

        // Act
        var groupToDelete = await DebtTrackerDbContextSUT.Groups
            .SingleAsync(i => i.Id == GroupSeeds.GroupToDeleteCascadeEntity.Id);
        DebtTrackerDbContextSUT.Groups.Remove(groupToDelete);
        await DebtTrackerDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualGroup = await dbx.Groups.SingleOrDefaultAsync(i => i.Id == GroupSeeds.GroupToDeleteCascadeEntity.Id);
        Assert.Null(actualGroup);
        
        var actualDebts = await dbx.Debts.Where(i => i.GroupId == GroupSeeds.GroupToDeleteCascadeEntity.Id).ToListAsync();
        Assert.Empty(actualDebts);
        
        var actualRegistrations = await dbx.RegisteredGroups.Where(i => i.GroupId == GroupSeeds.GroupToDeleteCascadeEntity.Id).ToListAsync();
        Assert.Empty(actualRegistrations);
    }
}