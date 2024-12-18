﻿using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using SomeSandwich.Donut.Application.Common.JsonConverters;

namespace SomeSandwich.Donut.Application.Common;

/// <summary>
/// A class that provides metadata for (de)serializing JSON for both the API endpoints and with OpenAPI.
/// </summary>
[JsonSerializable(typeof(Guid))]
[JsonSerializable(typeof(JsonObject))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = true,
    Converters = [typeof(BsonDocumentJsonConverter)])]
public sealed partial class OpenApiJsonSerializerContext : JsonSerializerContext;
