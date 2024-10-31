using Gateway.API.BackgroundServices;
using Gateway.API.Constants;
using Gateway.API.Infrastructure.Startup;
using Serilog;
using Steeltoe.Discovery.Client;

namespace Gateway.API;

internal sealed class Program
{
    /// <summary>
    /// The entry point of the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Services.AddLogging();
        builder.Services.AddDiscoveryClient();
        builder.Services.AddReverseProxy().LoadFromMemory(YarpRoutes.Routes, YarpRoutes.DefaultClusters);
        builder.Services.AddHostedService<UpdateClusterDestinationService>();
        builder.Services.AddHealthChecks();

        // Logging.
        builder.Services.AddSerilog(new SerilogConfiguration(configuration).Setup);

        var app = builder.Build();
        app.UseHttpsRedirection();
        app.MapReverseProxy();
        app.MapHealthChecks("/healthz");
        app.Run();
    }
}
