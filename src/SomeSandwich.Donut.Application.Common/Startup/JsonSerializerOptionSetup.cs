using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using SomeSandwich.Donut.Abstractions.JsonConverters;

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
        options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.SerializerOptions.Converters.Add(new BsonDocumentJsonConverter());
        options.SerializerOptions.Converters.Add(new ObjectIdJsonConverter());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public JsonSerializerOptions Setup(JsonSerializerOptions options)
    {
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.Converters.Add(new BsonDocumentJsonConverter());
        options.Converters.Add(new ObjectIdJsonConverter());

        return options;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static JsonSerializerOptions Get()
    {
        return new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new BsonDocumentJsonConverter(), new ObjectIdJsonConverter() }
        };
    }
}
