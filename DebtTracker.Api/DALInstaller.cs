using CubiTracker.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;
using DebtTracker.DAL.Factories.SqLite;
using DebtTracker.DAL.Mappers;
using DebtTracker.DAL.UnitOfWork;

namespace DebtTracker.DAL;

public static class DALInstaller
{
    public static IServiceCollection AddDALServices(
        this IServiceCollection services,
        string dbConnectionString,
        bool seedDemoData = false,
        bool recreateDb = false)
    {
        var dbContextSqLiteFactory = new SqLiteDbContextFactory(dbConnectionString, seedDemoData);
        services.AddSingleton<IDbContextFactory<DebtTrackerDbContext>>(dbContextSqLiteFactory);

        using var debtTrackerDbContext = dbContextSqLiteFactory.CreateDbContext();
        if (recreateDb)
        {
            debtTrackerDbContext.Database.EnsureDeleted();
        }
        debtTrackerDbContext.Database.EnsureCreated();

        services.Scan(scan => scan
            .FromAssemblyOf<DebtTrackerDbContext>()
            .AddClasses(filter => filter.AssignableTo(typeof(IEntityMapper<>)))
            .AsMatchingInterface()
            .WithSingletonLifetime());

        services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();

        return services;
    }
}