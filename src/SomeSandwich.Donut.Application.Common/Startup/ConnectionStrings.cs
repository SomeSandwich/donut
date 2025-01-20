namespace SomeSandwich.Donut.Application.Common.Startup;

/// <summary>
/// Represents the connection strings required for the application.
/// </summary>
public class ConnectionStrings
{
    /// <summary>
    /// Gets or sets the connection string for the LinkDB database.
    /// </summary>
    public string LinkDB { get; set; }

    /// <summary>
    /// Gets or sets the connection string for the cache.
    /// </summary>
    public string Cache { get; set; }
}
