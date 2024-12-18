namespace SomeSandwich.Donut.Abstractions.OpenApi.Atributes;

/// <summary>
/// An attribute representing an example for an OpenAPI ID operation parameter.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class OpenApiIdExampleAttribute : OpenApiExampleAttribute<Guid, OpenApiIdExampleAttribute>, IExampleProvider<Guid>
{
    private static readonly Guid Value = Guid.Parse("a03952ca-880e-4af7-9cfa-630be0feb4a5");

    /// <inheritdoc/>
    static Guid IExampleProvider<Guid>.GenerateExample() => Value;

    /// <inheritdoc />
    public override Guid GenerateExample() => Value;
}
