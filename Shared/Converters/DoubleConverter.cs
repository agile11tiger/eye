using System.Globalization;
using System.Text.Json.Serialization;
namespace Memory.Converters;

/// <summary>
/// Example: "8.9"
/// </summary>
public class DoubleConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            && double.TryParse(reader.GetString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var value)
            ? value
            : reader.TokenType == JsonTokenType.Number
            ? reader.GetDouble()
            : default;
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
