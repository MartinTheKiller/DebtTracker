using System.ComponentModel.DataAnnotations;

namespace DebtTracker.BL.Models;

public record DebtListModel : ModelBase
{
    public required string Name { get; set; }
    public required uint Amount { get; set; }
    public required DateTime Date { get; set; }
    public DateTime? ResolvedDate { get; set; }

    public required Guid DebtorId { get; set; }
    public required Guid CreditorId { get; set; }
    public required Guid GroupId { get; set; }

    public static DebtListModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Amount = 0,
        Date = DateTime.Now,
        ResolvedDate = null,
        DebtorId = Guid.Empty,
        CreditorId = Guid.Empty,
        GroupId = Guid.Empty,
    };
}