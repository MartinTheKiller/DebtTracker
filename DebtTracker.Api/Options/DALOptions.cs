namespace DebtTracker.Api.Options;

public record DALOptions
{
    public SqLiteOptions? SqLite { get; init; }
    public SqlServerOptions? SqlServer { get; init; }
}

public record SqLiteOptions
{
    public bool Enabled { get; init; }

    public string DatabaseName { get; init; } = null!;
    /// <summary>
    /// Deletes database before application startup
    /// </summary>
    public bool RecreateDatabaseEachTime { get; init; } = false;

    /// <summary>
    /// Seeds DemoData from DbContext on database creation.
    /// </summary>
    public bool SeedDemoData { get; init; } = false;
}

public record SqlServerOptions
{
    public bool Enabled { get; init; }
    public string ConnectionString { get; init; } = null!;
}