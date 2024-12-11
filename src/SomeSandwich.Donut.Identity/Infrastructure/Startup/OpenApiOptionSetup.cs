using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace SomeSandwich.Donut.Identity.Infrastructure.Startup;

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
        options.AddDocumentTransformer((doc, context, cancellationToken) =>
        {
            doc.Servers = new List<OpenApiServer> { new() { Url = "http://localhost:5101" } };

            return Task.CompletedTask;
        });
    }
}
