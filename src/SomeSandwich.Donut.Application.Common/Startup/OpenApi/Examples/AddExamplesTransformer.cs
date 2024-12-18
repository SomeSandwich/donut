using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace SomeSandwich.Donut.Application.Common.Startup.OpenApi.Examples;

/// <summary>
/// A class representing an operation processor that adds examples to API endpoints.
/// </summary>
public class AddExamplesTransformer : ExamplesProcessor, IOpenApiOperationTransformer, IOpenApiSchemaTransformer
{
    /// <inheritdoc />
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        Process(operation, context.Description);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        Process(schema, context.JsonTypeInfo.Type);

        return Task.CompletedTask;
    }
}
