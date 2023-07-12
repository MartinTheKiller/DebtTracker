using System.ComponentModel;
using DebtTracker.Api.Options;
using DebtTracker.DAL;
using DebtTracker.DAL.Factories.MySql;
using DebtTracker.DAL.Factories.SqLite;
using DebtTracker.DAL.Factories.SqlServer;
using DebtTracker.DAL.Mappers;
using DebtTracker.DAL.UnitOfWork;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;

namespace DebtTracker.Api;

public static class DALInstaller
{
    public static IServiceCollection AddDALServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        DALOptions options = new();
        configuration.GetSection("DebtTracker:DAL").Bind(options);

        if (options.SqLite is null && options.SqlServer is null && options.MySql is null)
            throw new InvalidOperationException("No persistence provider configured");

        if (!(options.SqLite?.Enabled == true
              ^ options.SqlServer?.Enabled == true
              ^ options.MySql?.Enabled == true))
        {
            throw new InvalidOperationException("Exactly one persistence provider must be enabled");
        }


        if (options.SqLite?.Enabled == true) ConfigureSqLite(options.SqLite, services);

        if (options.SqlServer?.Enabled == true) ConfigureSqlServer(options.SqlServer, services);

        if (options.MySql?.Enabled == true) ConfigureMySql(options.MySql, services);

        services.Scan(scan => scan
            .FromAssemblyOf<DebtTrackerDbContext>()
            .AddClasses(filter => filter.AssignableTo(typeof(IEntityMapper<>)))
            .AsMatchingInterface()
            .WithSingletonLifetime());

        services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();

        return services;


        void ConfigureSqLite(SqLiteOptions options, IServiceCollection services)
        {
            if (options.DatabaseName is null)
                throw new InvalidOperationException($"{nameof(options.DatabaseName)} is not set");

            var sqLiteDbContextFactory = new SqLiteDbContextFactory(options.DatabaseName, options.SeedDemoData);

            using var debtTrackerDbContext = sqLiteDbContextFactory.CreateDbContext();
            if (options.RecreateDatabaseEachTime)
            {
                debtTrackerDbContext.Database.EnsureDeleted();
            }
            debtTrackerDbContext.Database.EnsureCreated();

            services.AddSingleton<IDbContextFactory<DebtTrackerDbContext>>(sqLiteDbContextFactory);
        }

        void ConfigureSqlServer(SqlServerOptions options, IServiceCollection services)
        {
            if (options.ConnectionString is null)
                throw new InvalidOperationException($"{nameof(options.ConnectionString)} is not set");

            services.AddSingleton<IDbContextFactory<DebtTrackerDbContext>>(provider => new SqlServerDbContextFactory(options.ConnectionString));
        }

        void ConfigureMySql(MySqlOptions options, IServiceCollection services)
        {
            if (options.ConnectionString is null)
                throw new InvalidOperationException($"{nameof(options.ConnectionString)} is not set");

            if (options.ServerVersion is null)
                throw new InvalidOperationException($"{nameof(options.ServerVersion)} is not set");

            services.AddSingleton<IDbContextFactory<DebtTrackerDbContext>>(provider => new MySqlDbContextFactory(options.ConnectionString, options.ServerVersion, options.UseMariaDbServer));
        }
    }
}