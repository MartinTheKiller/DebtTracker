using Xunit.Abstractions;
using DebtTracker.Common.Tests;
using DebtTracker.Common.Tests.Factories;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.DAL.Tests;

public class DbContextTestsBase : IAsyncLifetime
{
    protected DbContextTestsBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);
        
        DbContextFactory = new DbContextSQLiteTestingFactory(GetType().FullName!, seedTestingData: true);

        DebtTrackerDbContextSUT = DbContextFactory.CreateDbContext();
    }

    protected IDbContextFactory<DebtTrackerDbContext> DbContextFactory { get; }
    protected DebtTrackerDbContext DebtTrackerDbContextSUT { get; }


    public async Task InitializeAsync()
    {
        await DebtTrackerDbContextSUT.Database.EnsureDeletedAsync();
        await DebtTrackerDbContextSUT.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await DebtTrackerDbContextSUT.Database.EnsureDeletedAsync();
        await DebtTrackerDbContextSUT.DisposeAsync();
    }
}