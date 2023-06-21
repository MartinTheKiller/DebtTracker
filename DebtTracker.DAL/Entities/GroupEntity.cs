using System.ComponentModel.DataAnnotations;

namespace DebtTracker.DAL.Entities;

public record GroupEntity : IEntity
{
    public required Guid Id { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(500)]
    public string? PhotoUri { get; set; }

    public ICollection<RegisteredGroupEntity> Users { get; set; } = new List<RegisteredGroupEntity>();
    public ICollection<DebtEntity> Debts { get; set; } = new List<DebtEntity>();
}