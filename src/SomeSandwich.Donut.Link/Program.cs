using System.Reflection;
using MassTransit;
using Microsoft.AspNetCore.Http.Json;
using Newtonsoft.Json;
using Scalar.AspNetCore;
using Serilog;
using SomeSandwich.Donut.Application.Common.Extensions;
using SomeSandwich.Donut.Application.Common.Middlewares;
using SomeSandwich.Donut.Application.Common.Startup;
using SomeSandwich.Donut.Link.Infrastructure.DependencyInjection;
using Vault;
using Vault.Client;

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

        // Vault.
        var address = "http://vault:8200";
        var config = new VaultConfiguration(address);
        var client = new VaultClient(config);
        client.SetToken("root-token");
        services.AddSingleton<VaultClient>(_ => client);

        // Json Serialize and Deserialize settings.
        services.Configure<JsonOptions>(new JsonSerializerOptionSetup().Setup);

        // OpenAPI.
        services.AddOpenApi(new OpenApiOptionSetup(configuration).Setup);

        // Endpoints.
        services.AddEndpoints(Assembly.GetExecutingAssembly());

        // Database.
        var connectionString = JsonConvert.DeserializeObject<ConnectionStrings>(
            (await client.Secrets.KvV2ReadAsync("ConnectionStrings", "secret")).Data.Data.ToString() ?? throw new InvalidOperationException());
        services.AddMongoDB(connectionString.LinkDB);

        // Cache.
        services.AddFusionCache().Setup(connectionString.Cache);

        // Message Queue.
        var rabbitMqOptions = JsonConvert.DeserializeObject<RabbitMqOptions>(
            (await client.Secrets.KvV2ReadAsync("MessageQueue", "secret")).Data.Data.ToString() ?? throw new InvalidOperationException());
        services.AddMassTransit(new MassTransitConfiguratorSetup(rabbitMqOptions).Setup);

        // Logging.
        services.AddSerilog(new LoggingOptionsSetup(configuration).Setup);

        // Open Telemetry.
        services.AddOpenTelemetry().Setup("link-api", hasMongoDB: true, hasRedis: true, hasMassTransit: true);

        SystemModule.Register(services);

        var app = builder.Build();

        // Scalar.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(new ScalarOptionSetup().Setup);
        }

        // Custom middlewares.
        app.UseMiddleware<ApiExceptionMiddleware>();
        // app.UseSerilogRequestLogging(new LoggingOptionsSetup(configuration).SetupRequestLoggingOptions);

        app.MapEndpoints();

        await app.RunAsync();
    }
}
