using SomeSandwich.Donut.Application.Common.Startup.OpenApi;

namespace SomeSandwich.Donut.Link.Endpoints.Links;

/// <summary>
/// Represents a command to create a link.
/// </summary>
[OpenApiExample<CreateLinkCommand>]
public class CreateLinkCommand : IExampleProvider<CreateLinkCommand>
{
    /// <summary>
    /// Gets or sets the URL of the link to be created.
    /// </summary>
    public required string Url { get; init; }

    /// <inheritdoc />
    public static CreateLinkCommand GenerateExample()
    {
        return new CreateLinkCommand { Url = "https://www.example.com" };
    }
}
