using System.Runtime.CompilerServices;
using DebtTracker.BL.Facades;
using DebtTracker.BL.Hashers;
using DebtTracker.BL.Models;
using DebtTracker.BL.Models.User;
using DebtTracker.Common.Tests;
using DebtTracker.Common.Tests.Seeds;
using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace DebtTracker.BL.Tests;

public class UserFacadeTests : FacadeTestsBase
{
    private readonly UserFacade FacadeSUT;

    public UserFacadeTests(ITestOutputHelper output) : base(output)
    {
        IUserPasswordHasher passwordHasher = new UserPasswordHasher();
        FacadeSUT = new UserFacade(UnitOfWorkFactory, Mapper, passwordHasher);
    }

    [Fact]
    public async Task NewUser_EmptyId_Save_Added()
    {
        // Arrange
        var user = new UserCreateModel
        {
            Id = Guid.Empty,
            Name = "User",
            Surname = "New",
            Email = "usernew@new.com",
            HashedPassword = "$2a$10$CMRZOYCWjOa8qztdPYY3S.wsBZmLIIWyf3KQVbeQ3ccbBo9gqcRDG" //Password123
        };

        // Act
        var savedUser = await FacadeSUT.SaveAsync(user);

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users.SingleAsync(i => i.Id == savedUser.Id);
        DeepAssert.Equal(Mapper.Map<UserDetailModel>(user with { Id = savedUser.Id }), Mapper.Map<UserDetailModel>(actualEntity));
    }

    [Fact]
    public async Task NewUser_NewId_Save_Added()
    {
        // Arrange
        var user = new UserCreateModel
        {
            Id = Guid.NewGuid(),
            Name = "User",
            Surname = "New",
            Email = "usernew@new.com",
            HashedPassword = "$2a$10$CMRZOYCWjOa8qztdPYY3S.wsBZmLIIWyf3KQVbeQ3ccbBo9gqcRDG" //Password123
        };

        // Act
        var savedUser = await FacadeSUT.SaveAsync(user);

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users.SingleAsync(i => i.Id == savedUser.Id);
        DeepAssert.Equal(Mapper.Map<UserDetailModel>(user with { Id = savedUser.Id }), Mapper.Map<UserDetailModel>(actualEntity));
    }

    [Fact]
    public async Task NewUser_EmptyId_Create_Added()
    {
        // Arrange
        var user = new UserCreateModel
        {
            Id = Guid.Empty,
            Name = "User",
            Surname = "New",
            Email = "usernew@new.com",
            HashedPassword = "$2a$10$CMRZOYCWjOa8qztdPYY3S.wsBZmLIIWyf3KQVbeQ3ccbBo9gqcRDG" //Password123
        };

        // Act
        var savedUser = await FacadeSUT.CreateAsync(user);

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users.SingleAsync(i => i.Id == savedUser.Id);
        DeepAssert.Equal(Mapper.Map<UserDetailModel>(user with { Id = savedUser.Id }), Mapper.Map<UserDetailModel>(actualEntity));
    }

    [Fact]
    public async Task NewUser_NewId_Create_Added()
    {
        // Arrange
        var user = new UserCreateModel
        {
            Id = Guid.NewGuid(),
            Name = "User",
            Surname = "New",
            Email = "usernew@new.com",
            HashedPassword = "$2a$10$CMRZOYCWjOa8qztdPYY3S.wsBZmLIIWyf3KQVbeQ3ccbBo9gqcRDG" //Password123
        };

        // Act
        var savedUser = await FacadeSUT.CreateAsync(user);

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users.SingleAsync(i => i.Id == savedUser.Id);
        DeepAssert.Equal(Mapper.Map<UserDetailModel>(user with { Id = savedUser.Id }), Mapper.Map<UserDetailModel>(actualEntity));
    }

    [Fact]
    public async Task SeededUser_GetById_Exists()
    {
        // Act
        var actualUser = await FacadeSUT.GetAsync(UserSeeds.UserCreditorEntity.Id);

        // Assert
        Assert.NotNull(actualUser);
        DeepAssert.Equal(Mapper.Map<UserDetailModel>(UserSeeds.UserCreditorEntity), actualUser);
    }

    [Fact]
    public async Task SeededUser_UpdateById_Updated()
    {
        // Act
        var updatedUser = await FacadeSUT.GetAsync(UserSeeds.UserToUpdateEntity.Id);
        updatedUser.Name = "Updated";
        await FacadeSUT.UpdateAsync(updatedUser);
        UserSeeds.UserToUpdateEntity.Name = "Updated";

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualUser = await dbx.Users.SingleAsync(i => i.Id == UserSeeds.UserToUpdateEntity.Id);
        Assert.Equal(UserSeeds.UserToUpdateEntity.HashedPassword, actualUser.HashedPassword);
        DeepAssert.Equal(updatedUser, Mapper.Map<UserDetailModel>(actualUser));
    }

    [Fact]
    public async Task SeededUser_DeleteById_Deleted()
    {
        // Act
        await FacadeSUT.DeleteAsync(UserSeeds.UserToDeleteEntity.Id);

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualUser = await dbx.Users.SingleOrDefaultAsync(i => i.Id == UserSeeds.UserToDeleteEntity.Id);
        Assert.Null(actualUser);
    }
}