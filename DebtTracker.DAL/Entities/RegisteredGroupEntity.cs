using System.ComponentModel.DataAnnotations;

namespace DebtTracker.DAL.Entities;

public record RegisteredGroupEntity : IEntity
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public UserEntity? User { get; set; }
    public required Guid GroupId { get; set; }
    public GroupEntity? Group { get; set; }
}