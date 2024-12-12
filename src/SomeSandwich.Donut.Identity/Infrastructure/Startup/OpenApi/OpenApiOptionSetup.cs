using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace SomeSandwich.Donut.Identity.Infrastructure.Startup.OpenApi;

/// <summary>
/// Provides methods to configure OpenAPI options.
/// </summary>
public class OpenApiOptionSetup
{
    /// <summary>
    /// Configures the OpenAPI options by adding a document transformer.
    /// </summary>
    /// <param name="options">The OpenAPI options to configure.</param>
    public void Setup(OpenApiOptions options)
    {
        options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            document.Info.Title = "SomeSandwich Donut - Identity";
            document.Info.Description = "SomeSandwich Donut Identity API";
            document.Info.Version = "v1";

            document.Info.Contact = new OpenApiContact
            {
                Name = "Hieu Nguyen",
                Url = new Uri("https://github.com/SomeSandwich/donut")
            };

            document.Components ??= new OpenApiComponents();

            document.Servers = new List<OpenApiServer> { new() { Url = "http://localhost:5101" } };

            return Task.CompletedTask;
        });

        // Add a custom schema transformer to add descriptions from XML comments
        var descriptions = new AddSchemaDescriptionsTransformer();
        options.AddSchemaTransformer(descriptions);

        // Add transformer to add examples to OpenAPI parameters, requests, responses and schemas
        var examples = new AddExamplesTransformer();
        options.AddOperationTransformer(examples);
        options.AddSchemaTransformer(examples);
    }
}
