using Microsoft.OpenApi.Any;
using SomeSandwich.Donut.Abstractions.OpenApi.Extensions;

namespace SomeSandwich.Donut.Abstractions.OpenApi.Attributes;

/// <summary>
/// An attribute representing an example for an OpenAPI operation or schema.
/// </summary>
/// <typeparam name="TSchema">The type of the schema.</typeparam>
/// <typeparam name="TProvider">The type of the example provider.</typeparam>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
public class OpenApiExampleAttribute<TSchema, TProvider> : Attribute, IOpenApiExampleMetadata
    where TProvider : IExampleProvider<TSchema>
{
    /// <inheritdoc/>
    public Type SchemaType => typeof(TSchema);

    /// <summary>
    /// Generates the example to use.
    /// </summary>
    /// <returns>
    /// A <typeparamref name="TSchema"/> that should be used as the example.
    /// </returns>
    public virtual TSchema GenerateExample() => TProvider.GenerateExample();

    /// <inheritdoc/>
    object? IOpenApiExampleMetadata.GenerateExample() => GenerateExample();

    /// <inheritdoc/>
    IOpenApiAny IOpenApiExampleMetadata.GenerateExample(System.Text.Json.Serialization.JsonSerializerContext context)
        => ExampleFormatter.AsJson(GenerateExample(), context);
}
