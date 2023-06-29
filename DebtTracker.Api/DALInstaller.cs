using System.ComponentModel;
using DebtTracker.Api.Options;
using DebtTracker.DAL;
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

        if (options.SqLite is null && options.SqlServer is null)
        {
            throw new InvalidOperationException("No persistence provider configured");
        }

        if (!(options.SqLite?.Enabled == true
              ^ options.SqlServer?.Enabled == true))
        {
            throw new InvalidOperationException("Exactly one persistence provider must be enabled");
        }


        IDbContextFactory<DebtTrackerDbContext> dbContextFactory;

        if (options.SqLite?.Enabled == true)
        {
            if (options.SqLite.DatabaseName is null)
            {
                throw new InvalidOperationException($"{nameof(options.SqLite.DatabaseName)} is not set");
            }

            var sqLiteDbContextFactory = new SqLiteDbContextFactory(options.SqLite.DatabaseName, options.SqLite.SeedDemoData);

            using var debtTrackerDbContext = sqLiteDbContextFactory.CreateDbContext();
            if (options.SqLite.RecreateDatabaseEachTime)
            {
                debtTrackerDbContext.Database.EnsureDeleted();
            }
            debtTrackerDbContext.Database.EnsureCreated();

            services.AddSingleton<IDbContextFactory<DebtTrackerDbContext>>(sqLiteDbContextFactory);
        }

        if (options.SqlServer?.Enabled == true)
        {
            services.AddSingleton<IDbContextFactory<DebtTrackerDbContext>>(provider => new SqlServerDbContextFactory(options.SqlServer.ConnectionString));
        }

        services.Scan(scan => scan
            .FromAssemblyOf<DebtTrackerDbContext>()
            .AddClasses(filter => filter.AssignableTo(typeof(IEntityMapper<>)))
            .AsMatchingInterface()
            .WithSingletonLifetime());

        services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();

        return services;
    }
}