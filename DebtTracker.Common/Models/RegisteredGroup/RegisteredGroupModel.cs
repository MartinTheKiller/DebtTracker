namespace DebtTracker.Common.Models;

public record RegisteredGroupModel : ModelBase
{
    public required Guid UserId { get; set; }
    public UserListModel? User { get; set; }
    public required Guid GroupId { get; set; }
    public GroupListModel? Group { get; set; }

    public static RegisteredGroupModel Empty => new()
    {
        Id = Guid.Empty,
        UserId = Guid.Empty,
        User = UserListModel.Empty,
        GroupId = Guid.Empty,
        Group = GroupListModel.Empty
    };
}