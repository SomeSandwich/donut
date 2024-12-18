using System.Text.Json.Serialization;
using Microsoft.OpenApi.Any;

namespace SomeSandwich.Donut.Abstractions.OpenApi;

/// <summary>
/// Defines metadata to generate an OpenAPI example.
/// </summary>
public interface IOpenApiExampleMetadata
{
    /// <summary>
    /// Gets the type of the schema associated with the example.
    /// </summary>
    Type SchemaType { get; }

    /// <summary>
    /// Generates an example object for the schema.
    /// </summary>
    /// <returns>
    /// The example to use.
    /// </returns>
    object? GenerateExample();

    /// <summary>
    /// Generates an OpenAPI example for the schema.
    /// </summary>
    /// <param name="context">The JSON serializer context to use to generate the example.</param>
    /// <returns>
    /// The OpenAPI example to use.
    /// </returns>
    IOpenApiAny GenerateExample(JsonSerializerContext context);
}
