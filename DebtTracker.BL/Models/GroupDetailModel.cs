using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace DebtTracker.BL.Models;

public record GroupDetailModel : ModelBase
{
    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(500)]
    public string? PhotoUri { get; set; }
    public ObservableCollection<RegisteredGroupModel> Users { get; set; } = new();
    public ObservableCollection<DebtListModel> Debts { get; set; } = new();

    public static GroupDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        PhotoUri = string.Empty
    };
}