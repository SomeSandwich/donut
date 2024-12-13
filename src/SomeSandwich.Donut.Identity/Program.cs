using System.Reflection;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using SomeSandwich.Donut.Application.Common.Extensions;
using SomeSandwich.Donut.Application.Common.Startup;
using SomeSandwich.Donut.Application.Common.Startup.OpenApi;

namespace SomeSandwich.Donut.Identity;

/// <summary>
/// Entry point class.
/// </summary>
public static class Program
{
    /// <summary>
    /// Entry point method.
    /// </summary>
    /// <param name="args">Program arguments.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi(new OpenApiOptionSetup(AddApiDocumentInformation).Setup);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(new ScalarOptionSetup().Setup);
        }

        app.MapEndpoints();
        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.Run();
    }

    private static Task AddApiDocumentInformation(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Info.Title = "SomeSandwich Donut - Identity";
        document.Info.Description = "SomeSandwich Donut Identity API";
        document.Info.Version = "v1";

        document.Info.Contact = new OpenApiContact { Name = "SomeSandwich", Url = new Uri("https://github.com/SomeSandwich/donut") };

        document.Components ??= new OpenApiComponents();

        document.Servers = [new OpenApiServer { Url = "http://localhost:5101" }];

        return Task.CompletedTask;
    }
}
