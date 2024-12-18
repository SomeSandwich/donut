using Microsoft.AspNetCore.Http.Json;
using SomeSandwich.Donut.Application.Common.JsonConverters;

namespace SomeSandwich.Donut.Application.Common.Startup;

/// <summary>
/// Class responsible for setting up JSON serializer options.
/// </summary>
public class JsonSerializerOptionSetup
{
    /// <summary>
    /// Configures the provided <see cref="JsonOptions"/> with custom converters.
    /// </summary>
    /// <param name="options">The JSON options to configure.</param>
    public void Setup(JsonOptions options)
    {
        options.SerializerOptions.Converters.Add(new BsonDocumentJsonConverter());
    }
}
