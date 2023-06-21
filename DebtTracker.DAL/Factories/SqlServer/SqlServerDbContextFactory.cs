using Microsoft.EntityFrameworkCore;

namespace DebtTracker.DAL.Factories.SqlServer;

public class SqlServerDbContextFactory : IDbContextFactory<SqlServerDbContext>, IDbContextFactory<DebtTrackerDbContext>
{
    private readonly bool _seedDemoData;
    private readonly DbContextOptionsBuilder<DebtTrackerDbContext> _contextOptionsBuilder = new();

    public SqlServerDbContextFactory(string connectionString, bool seedDemoData = false)
    {
        _seedDemoData = seedDemoData;

        _contextOptionsBuilder.UseSqlServer(connectionString);

        ////Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        //_contextOptionsBuilder.LogTo(System.Console.WriteLine);
        //_contextOptionsBuilder.EnableSensitiveDataLogging();
    }

    public SqlServerDbContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedDemoData);
    DebtTrackerDbContext IDbContextFactory<DebtTrackerDbContext>.CreateDbContext() => CreateDbContext();
}