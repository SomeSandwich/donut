using Microsoft.AspNetCore.Routing;

namespace SomeSandwich.Donut.Application.Common.Interfaces;

/// <summary>
/// Defines a contract for mapping an endpoint to an endpoint route builder.
/// </summary>
public interface IEndpoint
{
    /// <summary>
    /// Maps the endpoint to the specified endpoint route builder.
    /// </summary>
    /// <param name="app">The endpoint route builder to map the endpoint to.</param>
    void MapEndpoint(IEndpointRouteBuilder app);
}
