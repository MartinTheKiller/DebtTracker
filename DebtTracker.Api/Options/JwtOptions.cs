namespace DebtTracker.Api.Options;

public record JwtOptions
{
    public bool Enabled { get; init; }
    public string Key { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public uint AccessTokenLifetime { get; init; }
}