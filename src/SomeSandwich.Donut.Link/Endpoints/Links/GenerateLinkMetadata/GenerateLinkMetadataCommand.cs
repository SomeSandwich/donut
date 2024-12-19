using MongoDB.Bson;
using SomeSandwich.Donut.Abstractions.OpenApi;
using SomeSandwich.Donut.Abstractions.OpenApi.Attributes;

namespace SomeSandwich.Donut.Link.Endpoints.Links.GenerateLinkMetadata;

/// <summary>
/// Command to generate metadata for a link.
/// </summary>
[OpenApiExample<GenerateLinkMetadataCommand>]
public class GenerateLinkMetadataCommand : IExampleProvider<GenerateLinkMetadataCommand>
{
    /// <summary>
    /// Gets or sets the ObjectId of the link.
    /// </summary>
    public ObjectId? Id { get; set; }

    /// <summary>
    /// Gets or sets the Href of the link.
    /// </summary>
    public required string Href { get; set; }

    /// <inheritdoc />
    public static GenerateLinkMetadataCommand GenerateExample()
    {
        return new GenerateLinkMetadataCommand { Href = "https://www.example.com" };
    }
}
