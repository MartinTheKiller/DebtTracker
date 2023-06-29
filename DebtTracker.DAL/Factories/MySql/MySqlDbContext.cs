using Microsoft.EntityFrameworkCore;

namespace DebtTracker.DAL.Factories.MySql;

public sealed class MySqlDbContext : DebtTrackerDbContext
{
    public MySqlDbContext(DbContextOptions contextOptions, bool seedDemoData = false) : base(contextOptions, seedDemoData)
    {
    }
}