﻿using System.Reflection;
using Dapr.Client;
using Scalar.AspNetCore;
using Serilog;
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
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        var services = builder.Services;
        var daprClient = new DaprClientBuilder().Build();

        // OpenAPI.
        services.AddOpenApi(new OpenApiOptionSetup(configuration).Setup);

        // Endpoints.
        services.AddEndpoints(Assembly.GetExecutingAssembly());

        // Logging.
        services.AddSerilog(new LoggingOptionsSetup(configuration).Setup);

        var app = builder.Build();

        // Scalar.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(new ScalarOptionSetup().Setup);
        }

        // Custom middleware.
        app.UseSerilogRequestLogging(new LoggingOptionsSetup(configuration).SetupRequestLoggingOptions);

        app.MapEndpoints();

        await app.RunAsync();
    }
}
