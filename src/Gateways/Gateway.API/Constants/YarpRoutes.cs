using Yarp.ReverseProxy.Configuration;

namespace Gateway.API.Constants;

/// <summary>
/// Represents the configuration for YARP routes.
/// </summary>
internal static class YarpRoutes
{
    /// <summary>
    /// Gets the array of route configurations.
    /// </summary>
    public static RouteConfig[] Routes =>
    [
        new()
        {
            RouteId = "identity-route",
            ClusterId = "IDENTITY-CLUSTER",
            Match = new RouteMatch { Path = "identity-api/{**catch-all}" },
            Transforms =
            [
                new Dictionary<string, string>
                {
                    { "PathPattern", "{**catch-all}" }
                }
            ]
        }
    ];

    /// <summary>
    /// Gets the array of cluster configurations.
    /// </summary>
    public static ClusterConfig[] DefaultClusters =>
    [
        new()
        {
            ClusterId = "IDENTITY-CLUSTER",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                { "identity-api-destination-1", new DestinationConfig { Address = "http://identity.api:8080" } }
            }
        }
    ];
}
