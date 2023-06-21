using DebtTracker.Common.Tests;
using DebtTracker.Common.Tests.Seeds;
using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace DebtTracker.DAL.Tests;

public class DbContextGroupTests : DbContextTestsBase
{
    public DbContextGroupTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task NewGroup_Add_Added()
    {
        // Arrange
        var group = new GroupEntity
        {
            Id = Guid.Parse("e2244917-b57b-4418-b15c-50d2af71ebef"),
            Name = "New group"
        };

        // Act
        DebtTrackerDbContextSUT.Groups.Add(group);
        await DebtTrackerDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualGroup = await dbx.Groups.SingleAsync(i => i.Id == group.Id);
        DeepAssert.Equal(group, actualGroup,
            $"{nameof(GroupEntity.Debts)}",
            $"{nameof(GroupEntity.Users)}");
    }

    [Fact]
    public async Task SeededGroup_GetById_Exists()
    {
        // Act
        var actualGroup = await DebtTrackerDbContextSUT.Groups
            .SingleOrDefaultAsync(i => i.Id == GroupSeeds.Group1Entity.Id);

        // Assert
        Assert.NotNull(actualGroup);
        DeepAssert.Equal(GroupSeeds.Group1Entity, actualGroup,
            $"{nameof(GroupEntity.Debts)}",
            $"{nameof(GroupEntity.Users)}");
    }

    [Fact]
    public async Task SeededGroup_UpdateById_Updated()
    {
        // Act
        var updatedGroup = await DebtTrackerDbContextSUT.Groups.SingleAsync(i => i.Id == GroupSeeds.GroupToUpdateEntity.Id);
        updatedGroup.Name = "Updated";
        await DebtTrackerDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualGroup = await dbx.Groups.SingleAsync(i => i.Id == GroupSeeds.GroupToUpdateEntity.Id);
        DeepAssert.Equal(updatedGroup, actualGroup,
            $"{nameof(GroupEntity.Debts)}",
            $"{nameof(GroupEntity.Users)}");
    }

    [Fact]
    public async Task SeededGroup_DeleteById_Deleted()
    {
        // Act
        var groupToDelete = await DebtTrackerDbContextSUT.Groups.SingleAsync(i => i.Id == GroupSeeds.GroupToDeleteEntity.Id);
        DebtTrackerDbContextSUT.Groups.Remove(groupToDelete);
        await DebtTrackerDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualGroup = await dbx.Groups.SingleOrDefaultAsync(i => i.Id == GroupSeeds.GroupToDeleteEntity.Id);
        Assert.Null(actualGroup);
    }
}