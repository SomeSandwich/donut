namespace SomeSandwich.Donut.Identity.Infrastructure.Startup.OpenApi;

/// <summary>
/// An attribute representing an example for an OpenAPI operation or schema.
/// </summary>
/// <typeparam name="T">The type of the schema.</typeparam>
public sealed class OpenApiExampleAttribute<T> : OpenApiExampleAttribute<T, T>
    where T : IExampleProvider<T>;
