namespace DebtTracker.BL.Models;

public record UserLoginModel : ModelBase
{
    public required string Email { get; init; }

    public static UserLoginModel Empty => new()
    {
        Id = Guid.Empty,
        Email = string.Empty
    };
}