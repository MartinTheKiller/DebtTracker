namespace DebtTracker.BL.Models;

public abstract record ModelBase : IModel
{
    public required Guid Id { get; init; }
}