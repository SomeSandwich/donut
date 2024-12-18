using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using SomeSandwich.Donut.Application.Common.Startup.OpenApi;
using SomeSandwich.Donut.Application.Common.Startup.OpenApi.Descriptions;
using SomeSandwich.Donut.Application.Common.Startup.OpenApi.Examples;

namespace SomeSandwich.Donut.Application.Common.Startup;

/// <summary>
/// Provides methods to configure OpenAPI options.
/// </summary>
public class OpenApiOptionSetup
{
    private readonly IConfiguration configuration;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="configuration">Configuration settings.</param>
    public OpenApiOptionSetup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    /// <summary>
    /// Configures the OpenAPI options by adding a document transformer.
    /// </summary>
    /// <param name="options">The OpenAPI options to configure.</param>
    public void Setup(OpenApiOptions options)
    {
        var information = configuration.GetSection("OpenApi").Get<OpenApiInformation>();

        options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            if (information is not null)
            {
                if (information.Title is not null)
                {
                    document.Info.Title = information.Title;
                }

                if (information.Description is not null)
                {
                    document.Info.Description = information.Description;
                }

                if (information.Version is not null)
                {
                    document.Info.Version = information.Version;
                }

                if (information.ServerUrl is not null)
                {
                    document.Servers = [new OpenApiServer { Url = information.ServerUrl }];
                }
            }

            document.Info.Contact = new OpenApiContact { Name = "SomeSandwich", Url = new Uri("https://github.com/SomeSandwich/donut") };
            document.Components ??= new OpenApiComponents();

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
