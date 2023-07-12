namespace DebtTracker.Common.ApiMessages;

public record AuthenticateResponse
{
    public required string AccessToken { get; init; }
}