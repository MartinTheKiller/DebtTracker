using DebtTracker.BL.Facades;
using DebtTracker.BL.Models;
using DebtTracker.BL.Models.Group;
using DebtTracker.Common.Tests;
using DebtTracker.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace DebtTracker.BL.Tests;

public class GroupFacadeTests : FacadeTestsBase
{
    private readonly GroupFacade FacadeSUT;

    public GroupFacadeTests(ITestOutputHelper output) : base(output)
    {
        FacadeSUT = new GroupFacade(UnitOfWorkFactory, Mapper);
    }

    [Fact]
    public async Task NewGroup_EmptyId_Save_Added()
    {
        // Arrange
        var group = new GroupDetailModel
        {
            Id = Guid.Empty,
            Name = "New group"
        };

        // Act
        var savedGroup = await FacadeSUT.SaveAsync(group);

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Groups.SingleAsync(i => i.Id == savedGroup.Id);
        DeepAssert.Equal(group with { Id = savedGroup.Id }, Mapper.Map<GroupDetailModel>(actualEntity));
    }

    [Fact]
    public async Task NewGroup_NewId_Save_Added()
    {
        // Arrange
        var group = new GroupDetailModel
        {
            Id = Guid.NewGuid(),
            Name = "New group",
        };

        // Act
        var savedGroup = await FacadeSUT.SaveAsync(group);

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Groups.SingleAsync(i => i.Id == savedGroup.Id);
        DeepAssert.Equal(group with { Id = savedGroup.Id }, Mapper.Map<GroupDetailModel>(actualEntity));
    }

    [Fact]
    public async Task SeededGroup_GetById_Exists()
    {
        // Act
        var actualGroup = await FacadeSUT.GetAsync(GroupSeeds.Group1Entity.Id);

        // Assert
        Assert.NotNull(actualGroup);
        DeepAssert.Equal(Mapper.Map<GroupDetailModel>(GroupSeeds.Group1Entity), actualGroup);
    }

    [Fact]
    public async Task SeededGroup_UpdateById_Updated()
    {
        // Act
        var updatedGroup = await FacadeSUT.GetAsync(GroupSeeds.GroupToUpdateEntity.Id);
        updatedGroup.Name = "Updated";
        await FacadeSUT.SaveAsync(updatedGroup);
        GroupSeeds.GroupToUpdateEntity.Name = "Updated";

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualGroup = await dbx.Groups.SingleAsync(i => i.Id == GroupSeeds.GroupToUpdateEntity.Id);
        DeepAssert.Equal(updatedGroup, Mapper.Map<GroupDetailModel>(actualGroup));
    }

    [Fact]
    public async Task SeededGroup_DeleteById_Deleted()
    {
        // Act
        await FacadeSUT.DeleteAsync(GroupSeeds.GroupToDeleteEntity.Id);

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualGroup = await dbx.Groups.SingleOrDefaultAsync(i => i.Id == GroupSeeds.GroupToDeleteEntity.Id);
        Assert.Null(actualGroup);
    }
}