using System.Reflection;
using System.Text.Json;
using Dapr.Client;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using Serilog;
using SomeSandwich.Donut.Application.Common.Extensions;
using SomeSandwich.Donut.Application.Common.JsonConverters;
using SomeSandwich.Donut.Application.Common.Middlewares;
using SomeSandwich.Donut.Application.Common.Startup;
using SomeSandwich.Donut.Link.Infrastructure.DependencyInjection;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

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

        // Json Serialize and Deserialize settings.
        services.Configure<JsonOptions>(new JsonSerializerOptionSetup().Setup);

        // Dapr.
        services.AddDaprClient();

        // OpenAPI.
        services.AddOpenApi(new OpenApiOptionSetup(configuration).Setup);

        // Endpoints.
        services.AddEndpoints(Assembly.GetExecutingAssembly());

        // Database.
        var dbConnectionString =
            (await daprClient.GetSecretAsync("donut-secrets", "ConnectionStrings:LinkDB"))["ConnectionStrings:LinkDB"];
        var clientSettings = MongoClientSettings.FromConnectionString(dbConnectionString);
        clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());
        services.AddSingleton<IMongoClient>(_ => new MongoClient(clientSettings));

        // Cache.
        var cacheConnectionString =
            (await daprClient.GetSecretAsync("donut-secrets", "ConnectionStrings:Cache"))["ConnectionStrings:Cache"];
        services.AddFusionCache()
            .WithDefaultEntryOptions(new FusionCacheEntryOptions()
                .SetDuration(TimeSpan.FromMinutes(2))
                .SetPriority(CacheItemPriority.High)
                .SetFailSafe(true, TimeSpan.FromHours(2))
                .SetFactoryTimeouts(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(2)))
            .WithSerializer(new FusionCacheSystemTextJsonSerializer(new JsonSerializerOptions()
            {
                Converters = { new BsonDocumentJsonConverter() }
            }))
            .WithDistributedCache(new RedisCache(new RedisCacheOptions { Configuration = cacheConnectionString }));

        // Logging.
        services.AddSerilog(new LoggingOptionsSetup(configuration).Setup);

        // Open Telemetry.
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("link-api"))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSource("MongoDB.Driver.Core.Extensions.DiagnosticSources") // For MongoDB.
                    .AddFusionCacheInstrumentation();

                tracing.AddOtlpExporter();
            });

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
        app.UseSerilogRequestLogging(new LoggingOptionsSetup(configuration).SetupRequestLoggingOptions);

        app.MapEndpoints();

        await app.RunAsync();
    }
}
