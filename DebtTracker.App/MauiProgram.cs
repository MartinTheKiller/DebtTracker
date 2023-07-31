using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using DebtTracker.App.Api;
using DebtTracker.App.Options;
using DebtTracker.App.Services;
using DebtTracker.App.ViewModels;
using DebtTracker.App.Views;
using Microsoft.Extensions.Options;

namespace DebtTracker.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        ConfigureAppSettings(builder);
        ConfigureOptions(builder);

        //ConfigureShell(builder.Services);
        ConfigureViews(builder.Services);
        ConfigureViewModels(builder.Services);

        ConfigureServices(builder.Services);
        ConfigureApiClients(builder.Services);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        //RegisterRoutes(app);

        return app;
    }


    private static void ConfigureAppSettings(MauiAppBuilder builder)
    {
        var configurationBuilder = new ConfigurationBuilder();

        var assembly = Assembly.GetExecutingAssembly();
        var appSettingsFilePath = "DebtTracker.App.Configuration.appsettings.json";
        using var appSettingsStream = assembly.GetManifestResourceStream(appSettingsFilePath);
        if (appSettingsStream is not null)
        {
            configurationBuilder.AddJsonStream(appSettingsStream);
        }

        var configuration = configurationBuilder.Build();
        builder.Configuration.AddConfiguration(configuration);
    }

    private static void ConfigureOptions(MauiAppBuilder builder)
    {
        builder.Services.Configure<ApiOptions>(options =>
            builder.Configuration.GetSection(nameof(ApiOptions)).Bind(options));
    }

    private static void ConfigureViews(IServiceCollection services)
    {
        services.Scan(selector => selector
            .FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<ContentPageBase>())
            .AsSelf()
            .WithTransientLifetime());
    }

    private static void ConfigureViewModels(IServiceCollection services)
    {
        services.Scan(selector => selector
            .FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<IViewModel>())
            .AsSelfWithInterfaces()
            .WithTransientLifetime());
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        IdentityService identityService = new();
        ApiClientConfiguration apiClientConfiguration = new()
        {
            IdentityService = identityService
        };

        services.AddSingleton(identityService);
        services.AddSingleton(apiClientConfiguration);
    }

    private static void ConfigureApiClients(IServiceCollection services)
    {
        services.AddHttpClient<IDebtsClient, DebtsClient>((serviceProvider, client) =>
        {
            var apiOptions = serviceProvider.GetRequiredService<IOptions<ApiOptions>>();
            client.BaseAddress = new Uri(apiOptions.Value.ApiUrl ?? throw new ArgumentNullException($"{nameof(apiOptions)}.{nameof(ApiOptions.ApiUrl)}"));
        });

        services.AddHttpClient<IGroupsClient, GroupsClient>((serviceProvider, client) =>
        {
            var apiOptions = serviceProvider.GetRequiredService<IOptions<ApiOptions>>();
            client.BaseAddress = new Uri(apiOptions.Value.ApiUrl ?? throw new ArgumentNullException($"{nameof(apiOptions)}.{nameof(ApiOptions.ApiUrl)}"));
        });

        services.AddHttpClient<IRegisteredGroupsClient, RegisteredGroupsClient>((serviceProvider, client) =>
        {
            var apiOptions = serviceProvider.GetRequiredService<IOptions<ApiOptions>>();
            client.BaseAddress = new Uri(apiOptions.Value.ApiUrl ?? throw new ArgumentNullException($"{nameof(apiOptions)}.{nameof(ApiOptions.ApiUrl)}"));
        });

        services.AddHttpClient<IUsersClient, UsersClient>((serviceProvider, client) =>
        {
            var apiOptions = serviceProvider.GetRequiredService<IOptions<ApiOptions>>();
            client.BaseAddress = new Uri(apiOptions.Value.ApiUrl ?? throw new ArgumentNullException($"{nameof(apiOptions)}.{nameof(ApiOptions.ApiUrl)}"));
        });

        services.AddHttpClient<IAuthenticationClient, AuthenticationClient>((serviceProvider, client) =>
        {
            var apiOptions = serviceProvider.GetRequiredService<IOptions<ApiOptions>>();
            client.BaseAddress = new Uri(apiOptions.Value.ApiUrl ?? throw new ArgumentNullException($"{nameof(apiOptions)}.{nameof(ApiOptions.ApiUrl)}"));
        });
    }
}