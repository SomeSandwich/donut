﻿using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using SomeSandwich.Donut.Abstractions.JsonConverters;

namespace SomeSandwich.Donut.Abstractions.OpenApi.Extensions;

/// <summary>
/// A class containing methods to help format JSON examples for OpenAPI.
/// </summary>
public static class ExampleFormatter
{
    private static JsonSerializerOptions jsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        Converters = { new BsonDocumentJsonConverter(), new ObjectIdJsonConverter() }
    };

    /// <summary>
    /// Formats the example for the specified type.
    /// </summary>
    /// <typeparam name="TSchema">The type of the schema.</typeparam>
    /// <typeparam name="TProvider">The type of the example provider.</typeparam>
    /// <param name="context">The JSON serializer context to use.</param>
    /// <returns>
    /// The <see cref="IOpenApiAny"/> to use as the example.
    /// </returns>
    public static IOpenApiAny AsJson<TSchema, TProvider>(JsonSerializerContext context)
        where TProvider : IExampleProvider<TSchema>
        => AsJson(TProvider.GenerateExample(), context);

    /// <summary>
    /// Formats the specified value as JSON.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="example">The example value to format as JSON.</param>
    /// <param name="context">The JSON serializer context to use.</param>
    /// <returns>
    /// The <see cref="IOpenApiAny"/> to use as the example.
    /// </returns>
    public static IOpenApiAny AsJson<T>(T example, JsonSerializerContext context)
    {
        // Apply any formatting rules configured for the API (e.g. camel casing)
        string json;
        if (typeof(T) == typeof(Guid) || typeof(T) == typeof(JsonObject) || typeof(T) == typeof(ProblemDetails))
        {
            json = JsonSerializer.Serialize(example, typeof(T), context);
        }
        else
        {
            json = JsonSerializer.Serialize(example, jsonSerializerOptions);
        }
        using var document = JsonDocument.Parse(json);

        if (document.RootElement.ValueKind == JsonValueKind.String)
        {
            return new OpenApiString(document.RootElement.ToString());
        }

        var result = new OpenApiObject();

        // Recursively build up the example from the properties of the object
        foreach (var token in document.RootElement.EnumerateObject())
        {
            if (TryParse(token.Value, out var any))
            {
                result[token.Name] = any;
            }
        }

        return result;
    }

    private static bool TryParse(JsonElement token, out IOpenApiAny? any)
    {
        any = null;

        switch (token.ValueKind)
        {
            case JsonValueKind.Array:
                var array = new OpenApiArray();

                foreach (var value in token.EnumerateArray())
                {
                    if (TryParse(value, out var child))
                    {
                        array.Add(child);
                    }
                }

                any = array;
                return true;

            case JsonValueKind.False:
                any = new OpenApiBoolean(false);
                return true;

            case JsonValueKind.True:
                any = new OpenApiBoolean(true);
                return true;

            case JsonValueKind.Number:
                any = new OpenApiDouble(token.GetDouble());
                return true;

            case JsonValueKind.String:
                any = new OpenApiString(token.GetString());
                return true;

            case JsonValueKind.Object:
                var obj = new OpenApiObject();

                foreach (var child in token.EnumerateObject())
                {
                    if (TryParse(child.Value, out var value))
                    {
                        obj[child.Name] = value;
                    }
                }

                any = obj;
                return true;

            case JsonValueKind.Null:
            case JsonValueKind.Undefined:
            default:
                return false;
        }
    }
}
