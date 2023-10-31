using System.Globalization;
using System.Text.Json.Serialization;
namespace Memory.Converters;

/// <summary>
/// Example: "17,489"
/// </summary>
public class Int32Converter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            && int.TryParse(reader.GetString(), NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var value)
            ? value
            : reader.TokenType == JsonTokenType.Number
            ? reader.GetInt32()
            : default;
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
