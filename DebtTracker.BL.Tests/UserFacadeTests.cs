using System.Runtime.CompilerServices;
using DebtTracker.BL.Facades;
using DebtTracker.BL.Hashers;
using DebtTracker.Common.Models;
using DebtTracker.Common.Tests;
using DebtTracker.Common.Tests.Seeds;
using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace DebtTracker.BL.Tests;

public class UserFacadeTests : FacadeTestsBase
{
    private readonly UserFacade FacadeSUT;
    private readonly IUserPasswordHasher _passwordHasher;

    public UserFacadeTests(ITestOutputHelper output) : base(output)
    {
        _passwordHasher = new UserPasswordHasher();
        FacadeSUT = new UserFacade(UnitOfWorkFactory, Mapper, _passwordHasher);
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
            HashedPassword = _passwordHasher.HashPassword(UserSeeds.DefaultPassword)
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
            HashedPassword = _passwordHasher.HashPassword(UserSeeds.DefaultPassword)
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
            HashedPassword = _passwordHasher.HashPassword(UserSeeds.DefaultPassword)
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
            HashedPassword = _passwordHasher.HashPassword(UserSeeds.DefaultPassword)
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
        updatedUser!.Name = "Updated";
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

    [Fact]
    public async Task SeededUser_CorrectLogin_UserId()
    {
        // Act
        var userId = await FacadeSUT.LoginAsync(UserSeeds.UserCreditorEntity.Email, UserSeeds.DefaultPassword);

        // Assert
        Assert.NotNull(userId);
        Assert.Equal(UserSeeds.UserCreditorEntity.Id, userId);
    }

    [Fact]
    public async Task SeededUser_IncorrectLogin_Null()
    {
        // Act
        var userId = await FacadeSUT.LoginAsync(UserSeeds.UserCreditorEntity.Email, "Wrong" + UserSeeds.DefaultPassword);

        // Assert
        Assert.Null(userId);
    }

    [Fact]
    public async Task SeededUser_ChangePasswordCorrectOldPassword_PasswordChanged()
    {
        // Act
        const string newPassword = "New" + UserSeeds.DefaultPassword;
        var result = await FacadeSUT.ChangePasswordAsync(UserSeeds.UserToChangePasswordEntity.Id, UserSeeds.DefaultPassword, newPassword);
        
        // Assert
        Assert.True(result);
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualUser = await dbx.Users.SingleAsync(i => i.Id == UserSeeds.UserToChangePasswordEntity.Id);
        Assert.True(_passwordHasher.VerifyHashedPassword(newPassword, actualUser.HashedPassword));
    }

    [Fact]
    public async Task SeededUser_ChangePasswordIncorrectOldPassword_PasswordNotChanged()
    {
        // Act
        const string newPassword = "New" + UserSeeds.DefaultPassword;
        var result = await FacadeSUT.ChangePasswordAsync(UserSeeds.UserToChangePasswordEntity.Id, "Wrong" + UserSeeds.DefaultPassword, newPassword);

        // Assert
        Assert.False(result);
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualUser = await dbx.Users.SingleAsync(i => i.Id == UserSeeds.UserToChangePasswordEntity.Id);
        Assert.True(_passwordHasher.VerifyHashedPassword(UserSeeds.DefaultPassword, actualUser.HashedPassword));
    }
}