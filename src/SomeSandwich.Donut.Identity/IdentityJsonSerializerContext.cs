using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace SomeSandwich.Donut.Identity;

/// <summary>
/// A class that provides metadata for (de)serializing JSON for both the API endpoints and with OpenAPI.
/// </summary>
[JsonSerializable(typeof(Guid))]
[JsonSerializable(typeof(JsonObject))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = true)]
public sealed partial class IdentityJsonSerializerContext : JsonSerializerContext;
