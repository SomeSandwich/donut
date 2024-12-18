using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace SomeSandwich.Donut.Application.Common.Utils;

/// <summary>
/// Utility class for adding metadata to route handlers.
/// </summary>
public static class EndpointUtil
{
    /// <summary>
    /// Adds metadata to the specified route handler.
    /// </summary>
    /// <typeparam name="T">The type of the response produced by the route handler.</typeparam>
    /// <param name="route">The route handler to which metadata will be added.</param>
    /// <param name="tag">The tag to associate with the route handler.</param>
    /// <param name="summary">The summary description of the route handler.</param>
    /// <returns>The route handler with the added metadata.</returns>
    public static RouteHandlerBuilder AddEndpointMetadata<T>(this RouteHandlerBuilder route, string tag = "", string summary = "")
    {
        return route
             .WithTags(tag)
             .WithSummary(summary)
             .Produces<T>();
    }
}
