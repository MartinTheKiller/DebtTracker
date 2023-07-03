namespace DebtTracker.BL.Models;

public record UserLoginModel
{
    public required string Email { get; init; }

    public static UserLoginModel Empty => new()
    {
        Email = string.Empty
    };
}