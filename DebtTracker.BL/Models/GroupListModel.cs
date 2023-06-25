using System.ComponentModel.DataAnnotations;

namespace DebtTracker.BL.Models;

public record GroupListModel : ModelBase
{
    public required string Name { get; set; }
    public string? PhotoUri { get; set; }

    public static GroupListModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        PhotoUri = string.Empty
    };
}