using System.ComponentModel.DataAnnotations;

namespace DebtTracker.DAL.Entities;

public record DebtEntity : IEntity
{
    public required Guid Id { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    public required uint Amount { get; set; }
    public required DateTime Date { get; set; }
    [MaxLength(500)]
    public string? Description { get; set; }
    public DateTime? ResolvedDate { get; set; }
    
    public required Guid DebtorId { get; set; }
    public UserEntity? Debtor { get; set; }
    public required Guid CreditorId { get; set; }
    public UserEntity? Creditor { get; set; }
    public required Guid GroupId { get; set; }
    public GroupEntity? Group { get; set;}
}