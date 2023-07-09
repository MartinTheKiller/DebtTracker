using System.Security.Claims;
using DebtTracker.Api.Identity;

namespace DebtTracker.Api.Handlers;

public static class HttpContextAuthenticationExtension 
{
    public static Guid GetUserId(this HttpContext context) =>
        Guid.TryParse(context.User.FindFirstValue(IdentityData.UserIdClaimName), out Guid userId)
        ? userId
        : throw new InvalidOperationException($"HttpContext does not include {IdentityData.UserIdClaimName} claim");

    public static string GetUserRole(this HttpContext context) =>
        context.User.FindFirstValue(ClaimTypes.Role)
        ?? throw new InvalidOperationException("HttpContext does not include Role claim");
}