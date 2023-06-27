using DebtTracker.DAL;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.Common.Tests.Factories;

public class DbContextSQLiteTestingFactory : IDbContextFactory<DebtTrackerDbContext>
{
    private readonly string _databaseName;
    private readonly bool _seedTestingData;

    public DbContextSQLiteTestingFactory(string databaseName, bool seedTestingData = false)
    {
        _databaseName = databaseName;
        _seedTestingData = seedTestingData;
    }
    public DebtTrackerDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<DebtTrackerDbContext> builder = new();
        builder.UseSqlite($"Data Source={_databaseName};Cache=Shared");

        //builder.LogTo(Console.WriteLine);       //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        //builder.EnableSensitiveDataLogging();

        return new DebtTrackerTestingDbContext(builder.Options, _seedTestingData);
    }
}