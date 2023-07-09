using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using DebtTracker.BL.Models.Debt;
using DebtTracker.BL.Models.RegisteredGroup;

namespace DebtTracker.BL.Models.User;

public record UserDetailModel : ModelBase
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

    public ObservableCollection<DebtListModel> OwesDebts { get; set; } = new();
    public ObservableCollection<DebtListModel> LentDebts { get; set; } = new();
    public ObservableCollection<RegisteredGroupModel> Groups { get; set; } = new();

    public static UserDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Surname = string.Empty,
        BankAccount = string.Empty,
        Email = string.Empty,
        PhotoUri = string.Empty
    };
}