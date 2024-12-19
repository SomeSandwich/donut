using SomeSandwich.Donut.Abstractions.OpenApi;
using SomeSandwich.Donut.Abstractions.OpenApi.Attributes;

namespace SomeSandwich.Donut.Link.Endpoints.Links.CreateLink;

/// <summary>
/// Represents a command to create a link.
/// </summary>
[OpenApiExample<CreateLinkCommand>]
public class CreateLinkCommand : IExampleProvider<CreateLinkCommand>
{
    /// <summary>
    /// Gets or sets the URL of the link to be created.
    /// </summary>
    public required string Href { get; init; }

    /// <inheritdoc />
    public static CreateLinkCommand GenerateExample()
    {
        return new CreateLinkCommand { Href = "https://www.example.com" };
    }
}
