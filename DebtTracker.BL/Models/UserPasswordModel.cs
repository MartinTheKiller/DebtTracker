using System.ComponentModel.DataAnnotations;

namespace DebtTracker.BL.Models;

public record UserPasswordModel : ModelBase
{
    [MaxLength(60)]
    public required string HashedPassword { get; init; } = string.Empty;

    public static UserPasswordModel Empty => new()
    {
        Id = Guid.Empty,
        HashedPassword = string.Empty
    };
}