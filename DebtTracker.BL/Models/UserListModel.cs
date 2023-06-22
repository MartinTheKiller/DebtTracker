using System.ComponentModel.DataAnnotations;

namespace DebtTracker.BL.Models;

public record UserListModel : ModelBase
{
    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(50)]
    public required string Surname { get; set; }
    [MaxLength(500)]
    public string? PhotoUri { get; set; }

    public static UserListModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Surname = string.Empty,
        PhotoUri = string.Empty
    };
}