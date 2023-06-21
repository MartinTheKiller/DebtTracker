using Microsoft.EntityFrameworkCore;

namespace DebtTracker.DAL.Factories.SqLite;

public sealed class SqLiteDbContext : DebtTrackerDbContext
{
    public SqLiteDbContext(DbContextOptions contextOptions, bool seedDemoData = false) : base(contextOptions, seedDemoData)
    {
    }
}