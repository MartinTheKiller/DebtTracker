using System.IdentityModel.Tokens.Jwt;

namespace DebtTracker.Api.Messages;

public record AuthenticateResponse
{
    public required string AccessToken { get; init; }
}