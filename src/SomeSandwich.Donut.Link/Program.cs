using System.Reflection;
using Dapr.Client;
using MongoDB.Driver;
using Scalar.AspNetCore;
using SomeSandwich.Donut.Application.Common.Extensions;
using SomeSandwich.Donut.Application.Common.Startup;
using SomeSandwich.Donut.Application.Common.Startup.OpenApi;

namespace SomeSandwich.Donut.Link;

/// <summary>
/// Entry point class.
/// </summary>
public class Program
{
    /// <summary>
    /// Entry point method.
    /// </summary>
    /// <param name="args">Program arguments.</param>
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        var services = builder.Services;
        var daprClient = new DaprClientBuilder().Build();

        // OpenAPI
        services.AddOpenApi(new OpenApiOptionSetup(configuration).Setup);

        // Endpoints
        services.AddEndpoints(Assembly.GetExecutingAssembly());

        // Database
        var dbConnectionString =
            (await daprClient.GetSecretAsync("donut-secrets", "ConnectionStrings:LinkDB"))["ConnectionStrings:LinkDB"];
        services.AddSingleton<IMongoClient>(_ => new MongoClient(dbConnectionString));

        var app = builder.Build();

        // Scalar
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(new ScalarOptionSetup().Setup);
        }

        app.MapEndpoints();

        await app.RunAsync();
    }
}
