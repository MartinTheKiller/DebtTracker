using System.ComponentModel.DataAnnotations;

namespace DebtTracker.BL.Models;

public record DebtDetailModel : ModelBase
{
    [MaxLength(50)]
    public required string Name { get; set; }
    public required uint Amount { get; set; }
    public required DateTime Date { get; set; }
    [MaxLength(500)]
    public string? Description { get; set; }
    public DateTime? ResolvedDate { get; set; }

    public required Guid DebtorId { get; set; }
    public UserListModel? Debtor { get; set; }
    public required Guid CreditorId { get; set; }
    public UserListModel? Creditor { get; set; }
    public required Guid GroupId { get; set; }
    public GroupListModel? Group { get; set; }

    public static DebtDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Amount = 0,
        Date = DateTime.Now,
        Description = string.Empty,
        ResolvedDate = null,
        DebtorId = Guid.Empty,
        Debtor = null,
        CreditorId = Guid.Empty,
        Creditor = null,
        GroupId = Guid.Empty,
        Group = null
    };
}