using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SomeSandwich.Donut.Abstractions.OpenApi;
using SomeSandwich.Donut.Abstractions.OpenApi.Attributes;

namespace SomeSandwich.Donut.Domain.Link;

/// <summary>
/// Represents a hyperlink with associated metadata.
/// </summary>
[OpenApiExample<Link>]
public class Link : IExampleProvider<Link>
{
    /// <summary>
    /// Gets or sets the unique identifier for the link.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// Gets or sets the URL of the link.
    /// </summary>
    public required string Href { get; set; }

    /// <summary>
    /// Gets or sets the metadata associated with the link.
    /// </summary>
    public BsonDocument Metadata { get; set; } = [];

    /// <summary>
    /// Generates an example instance of the <see cref="Link"/> class.
    /// </summary>
    /// <returns>
    /// A <see cref="Link"/> instance with example data.
    /// </returns>
    public static Link GenerateExample()
    {
        return new Link
        {
            Id = ObjectId.Empty,
            Href = "https://example.com",
            Metadata = new BsonDocument
            {
                { "IsSystemSave", true }
            }
        };
    }
}
