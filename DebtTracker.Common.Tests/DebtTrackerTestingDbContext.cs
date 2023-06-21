using DebtTracker.DAL;
using DebtTracker.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.Common.Tests;

public class DebtTrackerTestingDbContext : DebtTrackerDbContext
{
    private readonly bool _seedTestingData;

    public DebtTrackerTestingDbContext(DbContextOptions contextOptions, bool seedTestingData = false)
        : base(contextOptions, seedDemoData:false)
    {
        _seedTestingData = seedTestingData;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (_seedTestingData)
        {
            TestDataSeeder.Seed(modelBuilder);
        }
    }
}