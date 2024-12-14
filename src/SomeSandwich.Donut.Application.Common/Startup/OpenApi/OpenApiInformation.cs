namespace SomeSandwich.Donut.Application.Common.Startup.OpenApi;

/// <summary>
/// Represents the OpenAPI information for the application.
/// </summary>
public class OpenApiInformation
{
    /// <summary>
    /// Gets or initializes the title of the OpenAPI documentation.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Gets or initializes the description of the OpenAPI documentation.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets or initializes the version of the OpenAPI documentation.
    /// </summary>
    public string? Version { get; init; }

    /// <summary>
    /// Gets or initializes the server URL for the OpenAPI documentation.
    /// </summary>
    public string? ServerUrl { get; init; }
}
