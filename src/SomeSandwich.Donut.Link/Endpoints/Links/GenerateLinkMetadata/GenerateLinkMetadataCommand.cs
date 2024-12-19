using MongoDB.Bson;

namespace SomeSandwich.Donut.Link.Endpoints.Links.GenerateLinkMetadata;

/// <summary>
/// Command to generate metadata for a link.
/// </summary>
public class GenerateLinkMetadataCommand
{
    /// <summary>
    /// Gets or sets the ObjectId of the link.
    /// </summary>
    public ObjectId? Id { get; set; }

    /// <summary>
    /// Gets or sets the Href of the link.
    /// </summary>
    public required string Href { get; set; }
}
