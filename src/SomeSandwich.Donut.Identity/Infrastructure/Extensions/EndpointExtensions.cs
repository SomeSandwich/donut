using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SomeSandwich.Donut.Identity.Endpoints;

namespace SomeSandwich.Donut.Identity.Infrastructure.Extensions;

/// <summary>
/// Extension methods for adding and mapping endpoints.
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Adds all endpoints from the specified assembly to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the endpoints to.</param>
    /// <param name="assembly">The assembly to scan for endpoints.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] endpointServiceDescriptors = assembly.DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false }
                           && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(endpointServiceDescriptors);

        return services;
    }

    /// <summary>
    /// Maps all registered endpoints to the application's request pipeline.
    /// </summary>
    /// <param name="app">The application builder to map the endpoints to.</param>
    /// <param name="routeGroupBuilder">Optional route group builder for grouping endpoints.</param>
    /// <returns>The updated application builder.</returns>
    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder endpointRouteBuilder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(endpointRouteBuilder);
        }

        return app;
    }
}
