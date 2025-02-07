﻿using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Bson;

namespace SomeSandwich.Donut.Abstractions.JsonConverters;

/// <summary>
/// Converts a BsonDocument to and from JSON.
/// </summary>
public class BsonDocumentJsonConverter : JsonConverter<BsonDocument>
{
    /// <inheritdoc />
    public override BsonDocument? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        return BsonDocument.Parse(doc.RootElement.GetRawText());
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, BsonDocument value, JsonSerializerOptions options)
    {
        writer.WriteRawValue(value.ToJson());
    }
}
