using Microsoft.EntityFrameworkCore;

namespace DebtTracker.DAL.Factories.SqLite;

public class SqLiteDbContextFactory : IDbContextFactory<SqLiteDbContext>, IDbContextFactory<DebtTrackerDbContext>
{
    private readonly bool _seedDemoData;
    private readonly DbContextOptionsBuilder<DebtTrackerDbContext> _contextOptionsBuilder = new();

    public SqLiteDbContextFactory(string databaseName, bool seedDemoData = false)
    {
        _seedDemoData = seedDemoData;

        ////May be helpful for ad-hoc testing, not drop in replacement, needs some more configuration.
        //builder.UseSqlite($"Data Source =:memory:;");
        _contextOptionsBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");

        ////Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        //_contextOptionsBuilder.EnableSensitiveDataLogging();
        //_contextOptionsBuilder.LogTo(Console.WriteLine);
    }

    public SqLiteDbContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedDemoData);
    DebtTrackerDbContext IDbContextFactory<DebtTrackerDbContext>.CreateDbContext() => CreateDbContext();
}