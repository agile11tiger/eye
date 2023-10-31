using System.Text.Json.Serialization;
namespace Memory.Converters;

/// <summary>
/// Example: "2000".
/// Example(omdb): "2000–2005", "30 min".
/// </summary>
public class UInt16Converter : JsonConverter<ushort>
{
    public override ushort Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            && ushort.TryParse(reader.GetString()?.Split(' ', '–', '�').First(), out var value)
            ? value
            : reader.TokenType == JsonTokenType.Number
            ? reader.GetUInt16()
            : default;
    }

    public override void Write(Utf8JsonWriter writer, ushort value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
