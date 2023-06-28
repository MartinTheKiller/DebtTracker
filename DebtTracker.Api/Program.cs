using AutoMapper;
using DebtTracker.BL.Facades;
using DebtTracker.BL.Models;
using Microsoft.AspNetCore.Builder;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using NSwag.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

ConfigureOpenApiDocuments(builder.Services);
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var application = builder.Build();

ValidateAutoMapperConfiguration(application.Services);
UseSecurityFeatures(application);
UseRouting(application);
UseOpenApi(application);

application.Run();


void ConfigureOpenApiDocuments(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddOpenApiDocument(document =>
    {
        document.Title = "DebtTracker API";
        document.DocumentName = "debttracker-api";
    });
}

void ValidateAutoMapperConfiguration(IServiceProvider serviceProvider)
{
    var mapper = serviceProvider.GetRequiredService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
}

void UseSecurityFeatures(IApplicationBuilder app)
{
    app.UseHttpsRedirection();
}

void UseRouting(WebApplication app)
{
    app.MapGet("/", http =>
    {
        http.Response.Redirect("/swagger", false);
        return Task.CompletedTask;
    });

    UseDebtRouting(app);
    UseGroupRouting(app);
    UseRegisteredGroupRouting(app);
    UseUserRouting(app);


    void UseDebtRouting(WebApplication app)
    {
        const string DebtBasePath = "/DebtTrackerApi/Debts";
        const string DebtBaseName = "Debt";
        const string DebtsTag = $"{DebtBaseName}s";

        app.MapGet($"{DebtBasePath}", (IDebtFacade debtFacade) => debtFacade.GetAsync())
            .WithTags(DebtsTag)
            .WithName($"Get{DebtBaseName}sAll");

        app.MapGet($"{DebtBasePath}/{{id:guid}}", (Guid id, IDebtFacade debtFacade) => debtFacade.GetAsync(id))
            .WithTags(DebtsTag)
            .WithName($"Get{DebtBaseName}ById");

        app.MapPost($"{DebtBasePath}", (DebtDetailModel debt, IDebtFacade debtFacade) => debtFacade.SaveAsync(debt))
            .WithTags(DebtsTag)
            .WithName($"Create{DebtBaseName}");

        app.MapPut($"{DebtBasePath}", (DebtDetailModel debt, IDebtFacade debtFacade) => debtFacade.SaveAsync(debt))
            .WithTags(DebtsTag)
            .WithName($"Update{DebtBaseName}");

        app.MapDelete($"{DebtBasePath}/{{id:guid}}", (Guid id, IDebtFacade debtFacade) => debtFacade.DeleteAsync(id))
            .WithTags(DebtsTag)
            .WithName($"Delete{DebtBaseName}");
    }

    void UseGroupRouting(WebApplication app)
    {
        const string GroupBasePath = "/DebtTrackerApi/Groups";
        const string GroupBaseName = "Group";
        const string GroupsTag = $"{GroupBaseName}s";

        app.MapGet($"{GroupBasePath}", (IGroupFacade groupFacade) => groupFacade.GetAsync())
            .WithTags(GroupsTag)
            .WithName($"Get{GroupBaseName}sAll");

        app.MapGet($"{GroupBasePath}/{{id:guid}}", (Guid id, IGroupFacade groupFacade) => groupFacade.GetAsync(id))
            .WithTags(GroupsTag)
            .WithName($"Get{GroupBaseName}ById");

        app.MapPost($"{GroupBasePath}",
                (GroupDetailModel group, IGroupFacade groupFacade) => groupFacade.SaveAsync(group))
            .WithTags(GroupsTag)
            .WithName($"Create{GroupBaseName}");

        app.MapPut($"{GroupBasePath}",
                (GroupDetailModel group, IGroupFacade groupFacade) => groupFacade.SaveAsync(group))
            .WithTags(GroupsTag)
            .WithName($"Update{GroupBaseName}");

        app.MapDelete($"{GroupBasePath}/{{id:guid}}",
                (Guid id, IGroupFacade groupFacade) => groupFacade.DeleteAsync(id))
            .WithTags(GroupsTag)
            .WithName($"Delete{GroupBaseName}");
    }

    void UseRegisteredGroupRouting(WebApplication app)
    {
        const string RegisteredGroupBasePath = "/DebtTrackerApi/RegisteredGroups";
        const string RegisteredGroupBaseName = "RegisteredGroup";
        const string RegisteredGroupTag = $"{RegisteredGroupBaseName}s";

        app.MapGet($"{RegisteredGroupBasePath}", (IRegisteredGroupFacade registeredGroupFacade) => registeredGroupFacade.GetAsync())
            .WithTags(RegisteredGroupTag)
            .WithName($"Get{RegisteredGroupBaseName}sAll");

        app.MapGet($"{RegisteredGroupBasePath}/{{id:guid}}", (Guid id, IRegisteredGroupFacade registeredGroupFacade) => registeredGroupFacade.GetAsync(id))
            .WithTags(RegisteredGroupTag)
            .WithName($"Get{RegisteredGroupBaseName}ById");

        app.MapPost($"{RegisteredGroupBasePath}", (RegisteredGroupModel registeredGroup, IRegisteredGroupFacade registeredGroupFacade) => registeredGroupFacade.SaveAsync(registeredGroup))
            .WithTags(RegisteredGroupTag)
            .WithName($"Create{RegisteredGroupBaseName}");

        app.MapPut($"{RegisteredGroupBasePath}", (RegisteredGroupModel registeredGroup, IRegisteredGroupFacade registeredGroupFacade) => registeredGroupFacade.SaveAsync(registeredGroup))
            .WithTags(RegisteredGroupTag)
            .WithName($"Update{RegisteredGroupBaseName}");

        app.MapDelete($"{RegisteredGroupBasePath}/{{id:guid}}", (Guid id, IRegisteredGroupFacade registeredGroupFacade) => registeredGroupFacade.DeleteAsync(id))
            .WithTags(RegisteredGroupTag)
            .WithName($"Delete{RegisteredGroupBaseName}");
    }

    void UseUserRouting(WebApplication app)
    {
        const string UserBasePath = "/DebtTrackerApi/Users";
        const string UserBaseName = "User";
        const string UsersTag = $"{UserBaseName}s";

        app.MapGet($"{UserBasePath}", (IUserFacade userFacade) => userFacade.GetAsync())
            .WithTags(UsersTag)
            .WithName($"Get{UserBaseName}sAll");

        app.MapGet($"{UserBasePath}/{{id:guid}}", (Guid id, IUserFacade userFacade) => userFacade.GetAsync(id))
            .WithTags(UsersTag)
            .WithName($"Get{UserBaseName}ById");

        app.MapPost($"{UserBasePath}", (UserDetailModel user, IUserFacade userFacade) => userFacade.SaveAsync(user))
            .WithTags(UsersTag)
            .WithName($"Create{UserBaseName}");

        app.MapPut($"{UserBasePath}", (UserDetailModel user, IUserFacade userFacade) => userFacade.SaveAsync(user))
            .WithTags(UsersTag)
            .WithName($"Update{UserBaseName}");

        app.MapDelete($"{UserBasePath}/{{id:guid}}", (Guid id, IUserFacade userFacade) => userFacade.DeleteAsync(id))
            .WithTags(UsersTag)
            .WithName($"Delete{UserBaseName}");
    }
}

void UseOpenApi(IApplicationBuilder app)
{
    app.UseOpenApi();
    app.UseSwaggerUi3(settings =>
    {
        settings.DocumentTitle = "DebtTracker Swagger UI";
        settings.SwaggerRoutes.Add(new SwaggerUi3Route("DebtTracker API", "/swagger/debttracker-api/swagger.json"));
        settings.ValidateSpecification = true;
    });
}