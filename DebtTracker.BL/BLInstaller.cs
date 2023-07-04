using DebtTracker.BL.Facades;
using DebtTracker.BL.Hashers;
using Microsoft.Extensions.DependencyInjection;

namespace DebtTracker.BL;

public static class BLInstaller
{
    public static IServiceCollection AddBLServices(this IServiceCollection services)
    {
        services.AddSingleton<IDebtFacade, DebtFacade>();
        services.AddSingleton<IGroupFacade, GroupFacade>();
        services.AddSingleton<IRegisteredGroupFacade, RegisteredGroupFacade>();
        services.AddSingleton<IUserFacade, UserFacade>();

        services.AddSingleton<IUserPasswordHasher, UserPasswordHasher>();

        return services;
    }
}