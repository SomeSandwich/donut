using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace SomeSandwich.Donut.Application.Common.Startup.OpenApi;

/// <summary>
/// Provides methods to configure OpenAPI options.
/// </summary>
public class OpenApiOptionSetup
{
    private readonly Func<OpenApiDocument, OpenApiDocumentTransformerContext, CancellationToken, Task>? apiInformation;

    /// <summary>
    /// Constructor.
    /// </summary>
    public OpenApiOptionSetup()
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="apiInformation">A function to set up the OpenAPI document.</param>
    public OpenApiOptionSetup(Func<OpenApiDocument, OpenApiDocumentTransformerContext, CancellationToken, Task> apiInformation)
    {
        this.apiInformation = apiInformation;
    }

    /// <summary>
    /// Configures the OpenAPI options by adding a document transformer.
    /// </summary>
    /// <param name="options">The OpenAPI options to configure.</param>
    public void Setup(OpenApiOptions options)
    {
        if (apiInformation is not null)
        {
            options.AddDocumentTransformer(apiInformation);
        }

        // Add a custom schema transformer to add descriptions from XML comments
        var descriptions = new AddSchemaDescriptionsTransformer();
        options.AddSchemaTransformer(descriptions);

        // Add transformer to add examples to OpenAPI parameters, requests, responses and schemas
        var examples = new AddExamplesTransformer();
        options.AddOperationTransformer(examples);
        options.AddSchemaTransformer(examples);
    }
}
