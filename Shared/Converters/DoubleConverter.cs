using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EyE.Shared.Converters
{
    /// <summary>
    /// Example: "8.9"
    /// </summary>
    public class DoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String
                && double.TryParse(reader.GetString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var value))
                return value;
            else if (reader.TokenType == JsonTokenType.Number)
                return reader.GetDouble();

            return default;
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
