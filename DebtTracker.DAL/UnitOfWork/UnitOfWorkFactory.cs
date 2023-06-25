using Microsoft.EntityFrameworkCore;

namespace DebtTracker.DAL.UnitOfWork;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IDbContextFactory<DebtTrackerDbContext> _dbContextFactory;

    public UnitOfWorkFactory(IDbContextFactory<DebtTrackerDbContext> dbContextFactory) => 
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

    public IUnitOfWork Create() => new UnitOfWork(_dbContextFactory.CreateDbContext());
}