using DebtTracker.App.Services;

namespace DebtTracker.App.Api;

public record ApiClientConfiguration
{
    public IdentityService IdentityService { get; init; } = new();
}