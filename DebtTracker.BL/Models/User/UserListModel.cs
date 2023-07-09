namespace DebtTracker.BL.Models.User;

public record UserListModel : ModelBase
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public string? PhotoUri { get; set; }

    public static UserListModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Surname = string.Empty,
        PhotoUri = string.Empty
    };
}