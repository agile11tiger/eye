using System.Text.Json.Serialization;
namespace MemoryLib.Converters;

/// <summary>
/// Example: "True", "False"
/// </summary>
public class BooleanConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String && bool.TryParse(reader.GetString(), out var value)
            ? value
            : (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False) && reader.GetBoolean();
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}
