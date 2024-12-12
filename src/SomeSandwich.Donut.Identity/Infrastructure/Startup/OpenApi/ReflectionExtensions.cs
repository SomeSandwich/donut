using System.Reflection;

namespace SomeSandwich.Donut.Identity.Infrastructure.Startup.OpenApi;

public static class ReflectionExtensions
{
    public static IEnumerable<IOpenApiExampleMetadata> GetExampleMetadata(this MethodInfo method)
        => method.GetCustomAttributes().OfType<IOpenApiExampleMetadata>();

    public static IEnumerable<IOpenApiExampleMetadata> GetExampleMetadata(this ParameterInfo parameter)
        => parameter.GetCustomAttributes().OfType<IOpenApiExampleMetadata>();

    public static IEnumerable<IOpenApiExampleMetadata> GetExampleMetadata(this Type type)
        => type.GetCustomAttributes().OfType<IOpenApiExampleMetadata>();
}
