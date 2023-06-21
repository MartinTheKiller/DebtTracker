using DebtTracker.DAL.Entities;
using DebtTracker.DAL.Seeds;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.DAL;

public abstract class DebtTrackerDbContext : DbContext
{
    private readonly bool _seedDemoData;

    public DebtTrackerDbContext(DbContextOptions contextOptions, bool seedDemoData = false) : base(contextOptions) => 
        _seedDemoData = seedDemoData;

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<DebtEntity> Debts => Set<DebtEntity>();
    public DbSet<GroupEntity> Groups => Set<GroupEntity>();
    public DbSet<RegisteredGroupEntity> RegisteredGroups => Set<RegisteredGroupEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DebtEntity>()
            .HasOne(d => d.Debtor)
            .WithMany(u => u.OwesDebts)
            .HasForeignKey(d => d.DebtorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DebtEntity>()
            .HasOne(d => d.Creditor)
            .WithMany(u => u.LentDebts)
            .HasForeignKey(d => d.CreditorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DebtEntity>()
            .HasOne(d => d.Group)
            .WithMany(g => g.Debts)
            .HasForeignKey(d => d.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RegisteredGroupEntity>()
            .HasOne(r => r.User)
            .WithMany(u => u.Groups)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RegisteredGroupEntity>()
            .HasOne(r => r.Group)
            .WithMany(g => g.Users)
            .HasForeignKey(r => r.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        if (_seedDemoData)
        {
            DemoDataSeeder.Seed(modelBuilder);
        }
    }
}