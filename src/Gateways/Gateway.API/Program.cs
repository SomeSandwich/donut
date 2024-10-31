using Gateway.API.BackgroundServices;
using Gateway.API.Constants;
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
        builder.Services.AddLogging();
        builder.Services.AddDiscoveryClient();
        builder.Services.AddReverseProxy().LoadFromMemory(YarpRoutes.Routes, YarpRoutes.DefaultClusters);
        builder.Services.AddHostedService<UpdateClusterDestinationService>();

        builder.Services.AddHealthChecks();

        var app = builder.Build();
        app.UseHttpsRedirection();
        app.MapReverseProxy();
        app.MapHealthChecks("/healthz");
        app.Run();
    }
}
