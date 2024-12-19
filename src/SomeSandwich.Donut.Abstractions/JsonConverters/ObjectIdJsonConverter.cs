using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Bson;

namespace SomeSandwich.Donut.Abstractions.JsonConverters;

/// <inheritdoc />
public class ObjectIdJsonConverter : JsonConverter<ObjectId>
{
    /// <inheritdoc />
    public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return ObjectId.Parse(reader.GetString());
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
