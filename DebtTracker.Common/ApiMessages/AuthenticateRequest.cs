namespace DebtTracker.Common.ApiMessages;

public record AuthenticateRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }

    public static AuthenticateRequest Empty => new()
    {
        Email = string.Empty,
        Password = string.Empty
    };
}