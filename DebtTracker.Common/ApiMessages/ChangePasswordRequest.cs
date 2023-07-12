namespace DebtTracker.Common.ApiMessages;

public record ChangePasswordRequest
{
    public string OldPassword { get; init; } = null!;
    public string NewPassword { get; init; } = null!;
}