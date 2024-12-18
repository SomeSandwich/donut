using System.Reflection;
using Dapr.Client;
using Microsoft.AspNetCore.Http.Json;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using Serilog;
using SomeSandwich.Donut.Application.Common.Extensions;
using SomeSandwich.Donut.Application.Common.Middlewares;
using SomeSandwich.Donut.Application.Common.Startup;
using SomeSandwich.Donut.Identity.Infrastructure.DependencyInjection;

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

        // Logging.
        services.AddSerilog(new LoggingOptionsSetup(configuration).Setup);

        // Open Telemetry.
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("Identity.Api"))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

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
