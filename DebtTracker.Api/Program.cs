using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DebtTracker.BL.Facades;
using DebtTracker.BL.Models;
using System.Text.Json.Serialization;
using DebtTracker.Api;
using DebtTracker.Api.Handlers;
using DebtTracker.Api.Identity;
using DebtTracker.Api.Messages;
using DebtTracker.Api.Options;
using DebtTracker.BL;
using DebtTracker.BL.Models.Debt;
using DebtTracker.BL.Models.Group;
using DebtTracker.BL.Models.RegisteredGroup;
using DebtTracker.BL.Models.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using NSwag;

var builder = WebApplication.CreateBuilder(args);

ConfigureOpenApiDocuments(builder.Services, builder.Configuration);
builder.Services.AddBLServices()
                .AddDALServices(builder.Configuration)
                .AddAutoMapper(typeof(BLInstaller));
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
ConfigureSecurityFeatures(builder.Services, builder.Configuration);

var application = builder.Build();

ValidateAutoMapperConfiguration(application.Services);
UseRouting(application, builder.Configuration);
UseOpenApi(application);
UseSecurityFeatures(application, builder.Configuration); // UseSecurityFeatures is called after UseOpenApi to make swaggerUi accessible without authentication

application.Run();


void ConfigureOpenApiDocuments(IServiceCollection services, IConfiguration configuration)
{
    services.AddEndpointsApiExplorer();

    bool useJwt = bool.Parse(configuration["Jwt:Enabled"]!);

    services.AddOpenApiDocument(document =>
    {
        document.Title = "DebtTracker API";
        document.DocumentName = "debttracker-api";
        if (useJwt)
        {
            document.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Input: Bearer {your JWT token}."
            });

            document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        }
    });
}

void ConfigureSecurityFeatures(IServiceCollection services, IConfiguration configuration)
{
    JwtOptions jwtOptions = new();
    configuration.GetSection("Jwt").Bind(jwtOptions);

    if (!jwtOptions.Enabled) return;

    services.AddAuthentication(scheme =>
        {
            scheme.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            scheme.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
            });

    services.AddAuthorization(options =>
    {
        // All endpoints require authentication by default
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();
    });
}

void ValidateAutoMapperConfiguration(IServiceProvider serviceProvider)
{
    var mapper = serviceProvider.GetRequiredService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
}

void UseSecurityFeatures(IApplicationBuilder app, IConfiguration configuration)
{
    app.UseHttpsRedirection();

    bool useJwt = bool.Parse(configuration["Jwt:Enabled"]!);
    if (useJwt)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}

void UseRouting(WebApplication app, IConfiguration configuration)
{
    app.MapGet("/", http =>
    {
        http.Response.Redirect("/swagger", false);
        return Task.CompletedTask;
    }).AllowAnonymous();

    UseDebtRouting(app);
    UseGroupRouting(app);
    UseRegisteredGroupRouting(app);
    UseUserRouting(app);
    UseAuthenticationRouting(app, configuration);


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
            .WithName($"Save{DebtBaseName}");

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
            .WithName($"Save{GroupBaseName}");

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
            .WithName($"Save{RegisteredGroupBaseName}");

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

        app.MapPost($"{UserBasePath}", (UserCreateModel user, IUserFacade userFacade) => userFacade.CreateAsync(user))
            .WithTags(UsersTag)
            .WithName($"Create{UserBaseName}");

        app.MapPut($"{UserBasePath}", (UserDetailModel user, IUserFacade userFacade) => userFacade.UpdateAsync(user))
            .WithTags(UsersTag)
            .WithName($"Update{UserBaseName}");

        app.MapDelete($"{UserBasePath}/{{id:guid}}", (Guid id, IUserFacade userFacade) => userFacade.DeleteAsync(id))
            .WithTags(UsersTag)
            .WithName($"Delete{UserBaseName}");
    }

    void UseAuthenticationRouting(WebApplication app, IConfiguration configuration)
    {
        const string AuthenticationBasePath = "/DebtTrackerApi/Authentication";
        const string AuthenticationTag = "Authentication";

        JwtOptions jwtOptions = new();
        configuration.GetSection("Jwt").Bind(jwtOptions);

        if(!jwtOptions.Enabled) return;

        app.MapPost($"{AuthenticationBasePath}/", 
                async (AuthenticateRequest request, IUserFacade userFacade) => 
            {
                var loggedUserId = await userFacade.LoginAsync(request.Email, request.Password);
                if (loggedUserId is null)
                    return Results.BadRequest("Wrong email or password");
                
                var accessToken = new JwtSecurityToken
                (
                    issuer: jwtOptions.Issuer,
                    audience: jwtOptions.Audience,
                    claims: new[]
                    {
                        new Claim(IdentityData.UserIdClaimName, loggedUserId.ToString()!),
                        new Claim(ClaimTypes.Role, IdentityData.UserRoleClaimValue)
                    },
                    expires: DateTime.Now.AddSeconds(jwtOptions.AccessTokenLifetime),
                    notBefore: DateTime.Now,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)), SecurityAlgorithms.HmacSha256)
                );

                var response = new AuthenticateResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken)
                };

                return Results.Ok(response);
            })
            .WithTags(AuthenticationTag)
            .WithName($"Authenticate")
            .AllowAnonymous();

        app.MapPut($"{AuthenticationBasePath}/", (ChangePasswordRequest request, IUserFacade userFacade, HttpContext context) => 
                userFacade.ChangePasswordAsync(context.GetUserId(), request.OldPassword, request.NewPassword))
        .WithTags(AuthenticationTag)
        .WithName($"ChangePassword");
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