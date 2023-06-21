using Microsoft.EntityFrameworkCore;

namespace DebtTracker.DAL.Factories.SqlServer;

public sealed class SqlServerDbContext : DebtTrackerDbContext
{
    public SqlServerDbContext(DbContextOptions contextOptions, bool seedDemoData = false) : base(contextOptions, seedDemoData)
    {
    }
}