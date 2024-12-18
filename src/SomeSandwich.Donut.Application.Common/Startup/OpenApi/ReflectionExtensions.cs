using System.Reflection;
using SomeSandwich.Donut.Abstractions.OpenApi;

namespace SomeSandwich.Donut.Application.Common.Startup.OpenApi;

/// <summary>
/// Provides extension methods for reflection to retrieve OpenAPI example metadata.
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>
    /// Retrieves the OpenAPI example metadata from the specified method.
    /// </summary>
    /// <param name="method">The method to retrieve the metadata from.</param>
    /// <returns>An enumerable of <see cref="IOpenApiExampleMetadata"/>.</returns>
    public static IEnumerable<IOpenApiExampleMetadata> GetExampleMetadata(this MethodInfo method)
        => method.GetCustomAttributes().OfType<IOpenApiExampleMetadata>();

    /// <summary>
    /// Retrieves the OpenAPI example metadata from the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter to retrieve the metadata from.</param>
    /// <returns>An enumerable of <see cref="IOpenApiExampleMetadata"/>.</returns>
    public static IEnumerable<IOpenApiExampleMetadata> GetExampleMetadata(this ParameterInfo parameter)
        => parameter.GetCustomAttributes().OfType<IOpenApiExampleMetadata>();

    /// <summary>
    /// Retrieves the OpenAPI example metadata from the specified type.
    /// </summary>
    /// <param name="type">The type to retrieve the metadata from.</param>
    /// <returns>An enumerable of <see cref="IOpenApiExampleMetadata"/>.</returns>
    public static IEnumerable<IOpenApiExampleMetadata> GetExampleMetadata(this Type type)
        => type.GetCustomAttributes().OfType<IOpenApiExampleMetadata>();
}
