using DebtTracker.BL.Facades;
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

        return services;
    }
}