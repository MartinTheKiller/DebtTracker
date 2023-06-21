using System.ComponentModel.DataAnnotations;

namespace DebtTracker.DAL.Entities;

public record UserEntity : IEntity
{
    public required Guid Id { get; set; }
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

    public ICollection<DebtEntity> OwesDebts { get; set; } = new List<DebtEntity>();
    public ICollection<DebtEntity> LentDebts { get; set; } = new List<DebtEntity>();
    public ICollection<RegisteredGroupEntity> Groups { get; set; } = new List<RegisteredGroupEntity>();
}