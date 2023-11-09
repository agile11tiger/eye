using System.Text.Json.Serialization;
namespace MemoryLib.Converters;

/// <summary>
/// Example: "09 Sep 2000"
/// </summary>
public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String && DateTime.TryParse(reader.GetString(), out var value)
            ? value
            : default;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
