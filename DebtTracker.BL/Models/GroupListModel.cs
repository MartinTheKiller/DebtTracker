using System.ComponentModel.DataAnnotations;

namespace DebtTracker.BL.Models;

public record GroupListModel : ModelBase
{
    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(500)]
    public string? PhotoUri { get; set; }

    public static GroupListModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        PhotoUri = string.Empty
    };
}