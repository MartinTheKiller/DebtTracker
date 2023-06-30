using DebtTracker.DAL.Factories.MySql;
using DebtTracker.DAL.Factories.SqLite;
using DebtTracker.DAL.Factories.SqlServer;
using Microsoft.EntityFrameworkCore.Design;

namespace DebtTracker.DAL.Factories;

public class DesignTimeDbContextFactory 
    : IDesignTimeDbContextFactory<SqLiteDbContext>, IDesignTimeDbContextFactory<SqlServerDbContext>, IDesignTimeDbContextFactory<MySqlDbContext>
{
    private const string SqLiteDefaultDatabaseName = $"DebtTracker";
    private const string SqlServerDefaultConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DebtTracker;Integrated Security=True;";

    SqLiteDbContext IDesignTimeDbContextFactory<SqLiteDbContext>.CreateDbContext(string[] args)
    {
        if (args.Length > 1)
            throw new ArgumentException($"You must provide at most 1 argument: DatabaseName (\"{SqLiteDefaultDatabaseName}\" by default)");
        return new SqLiteDbContextFactory(args.Length == 1 ? args[0] : SqLiteDefaultDatabaseName).CreateDbContext();
    }

    SqlServerDbContext IDesignTimeDbContextFactory<SqlServerDbContext>.CreateDbContext(string[] args)
    {
        if (args.Length > 1)
            throw new ArgumentException($"You must provide at most 1 argument: ConnectionString (\"{SqlServerDefaultConnectionString}\" by default)");

        return new SqlServerDbContextFactory(args.Length == 1 ? args[0] : SqlServerDefaultConnectionString).CreateDbContext();
    }

    MySqlDbContext IDesignTimeDbContextFactory<MySqlDbContext>.CreateDbContext(string[] args)
    {
        if (args.Length != 3)
            throw new ArgumentException("You must provide 3 arguments: ConnectionString, ServerVersion, UseMariaDbServer");

        if(!bool.TryParse(args[2], out bool useMariaDbServer))
            throw new ArgumentException("Third argument must be \"true\" or \"false\"");

        return new MySqlDbContextFactory(args[0], args[1], useMariaDbServer).CreateDbContext();
    }
}