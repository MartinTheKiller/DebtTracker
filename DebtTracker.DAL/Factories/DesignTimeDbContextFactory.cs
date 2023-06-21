using DebtTracker.DAL.Factories.SqLite;
using DebtTracker.DAL.Factories.SqlServer;
using Microsoft.EntityFrameworkCore.Design;

namespace DebtTracker.DAL.Factories;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SqLiteDbContext>, IDesignTimeDbContextFactory<SqlServerDbContext>
{
    private readonly SqLiteDbContextFactory _dbContextSqLiteFactory;
    private const string SqLiteDatabaseName = $"DebtTracker";

    private readonly SqlServerDbContextFactory _dbContextSqlServerFactory;
    private const string SqlServerConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DebtTracker;Integrated Security=True;";

    public DesignTimeDbContextFactory()
    {
        _dbContextSqLiteFactory = new SqLiteDbContextFactory(SqLiteDatabaseName);
        _dbContextSqlServerFactory = new SqlServerDbContextFactory(SqlServerConnectionString);
    }

    SqLiteDbContext IDesignTimeDbContextFactory<SqLiteDbContext>.CreateDbContext(string[] args) => _dbContextSqLiteFactory.CreateDbContext();
    SqlServerDbContext IDesignTimeDbContextFactory<SqlServerDbContext>.CreateDbContext(string[] args) => _dbContextSqlServerFactory.CreateDbContext();
}