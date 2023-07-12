using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace DebtTracker.Common.Models;

public record UserCreateModel : ModelBase
{
    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(50)]
    public required string Surname { get; set; }
    [MaxLength(21)]
    public string? BankAccount { get; set; }    // 6 digit area code + 10 digit account number + '/' + 4 digit bank code
    [MaxLength(200)]
    public required string Email { get; set; }
    [MaxLength(500)]
    public string? PhotoUri { get; set; }
    [MaxLength(60)]
    public required string HashedPassword { get; init; } = string.Empty;

    public ObservableCollection<DebtListModel> OwesDebts { get; set; } = new();
    public ObservableCollection<DebtListModel> LentDebts { get; set; } = new();
    public ObservableCollection<RegisteredGroupModel> Groups { get; set; } = new();

    public static UserCreateModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Surname = string.Empty,
        BankAccount = string.Empty,
        Email = string.Empty,
        PhotoUri = string.Empty,
        HashedPassword = string.Empty
    };
}