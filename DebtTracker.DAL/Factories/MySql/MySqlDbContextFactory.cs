using DebtTracker.DAL.Factories.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.DAL.Factories.MySql;

public class MySqlDbContextFactory : IDbContextFactory<DebtTrackerDbContext>, IDbContextFactory<MySqlDbContext>
{
    private readonly bool _seedDemoData;
    private readonly DbContextOptionsBuilder<DebtTrackerDbContext> _contextOptionsBuilder = new();

    public MySqlDbContextFactory(string connectionString, string serverVersion, bool mariaDbServer, bool seedDemoData = false)
    {
        _seedDemoData = seedDemoData;

        if (mariaDbServer)
            _contextOptionsBuilder.UseMySql(connectionString, new MariaDbServerVersion(serverVersion));
        else
            _contextOptionsBuilder.UseMySql(connectionString, new MySqlServerVersion(serverVersion));

        ////Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        //_contextOptionsBuilder.LogTo(System.Console.WriteLine);
        //_contextOptionsBuilder.EnableSensitiveDataLogging();
    }

    public MySqlDbContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedDemoData);
    DebtTrackerDbContext IDbContextFactory<DebtTrackerDbContext>.CreateDbContext() => CreateDbContext();
}