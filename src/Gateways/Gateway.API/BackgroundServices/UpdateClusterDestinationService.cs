using System.Collections.Immutable;
using Gateway.API.Constants;
using Steeltoe.Discovery;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Discovery.Eureka.AppInfo;
using Yarp.ReverseProxy.Configuration;

namespace Gateway.API.BackgroundServices;

/// <summary>
/// Service responsible for updating cluster destinations based on registered applications.
/// </summary>
public sealed class UpdateClusterDestinationService : IHostedService, IDisposable
{
    private const int IntervalInSeconds = 30;

    private readonly ILogger<UpdateClusterDestinationService> logger;
    private readonly IServiceProvider services;
    private readonly DiscoveryClient client;
    private int executionCount;
    private Timer? timer;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="services">The service provider.</param>
    /// <param name="client">The discovery client.</param>
    public UpdateClusterDestinationService(ILogger<UpdateClusterDestinationService> logger, IServiceProvider services, IDiscoveryClient client)
    {
        this.logger = logger;
        this.services = services;
        this.client = (client as DiscoveryClient)!;
    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Update Cluster Destination Service running.");

        timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(IntervalInSeconds));

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Update Cluster Destination Service is stopping.");

        timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref executionCount);

        logger.LogInformation("Update Cluster Destination Service is working. Count: {Count}", count);

        var apps = client.Applications.GetRegisteredApplications();
        if (apps.Count == 0)
        {
            return;
        }

        var clusters = apps.Select(e =>
        {
            return new ClusterConfig
            {
                ClusterId = e.Name,
                Destinations = e.Instances
                    .Select(MapDestinationFromInstance)
                    .ToDictionary(valueTuple => valueTuple.InstanceId, i => i.DestinationConfig)
            };
        }).ToImmutableArray();

        var memoryProvider = services.GetRequiredService<InMemoryConfigProvider>();
        memoryProvider.Update(YarpRoutes.Routes, clusters);
    }

    private static (string InstanceId, DestinationConfig DestinationConfig) MapDestinationFromInstance(InstanceInfo instance)
    {
        return (instance.InstanceId, new DestinationConfig
        {
            Address = $"http://{instance.HostName}:{instance.Port}"
        });
    }

    #region Disposed

    private bool disposed;

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">True if call from Dispose method, false if call from finalizer.</param>
    private void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                timer?.Dispose();
            }

            disposed = true;
        }
    }

    #endregion
}
