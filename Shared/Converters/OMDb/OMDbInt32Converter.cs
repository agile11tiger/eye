using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EyE.Shared.Converters.OMDb
{
    /// <summary>
    /// Example: "17,489"
    /// </summary>
    public class OMDbInt32Converter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String
                && int.TryParse(reader.GetString(), NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var value))
                return value;
            else if (reader.TokenType == JsonTokenType.Number)
                return reader.GetInt32();

            return default;
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
