using System;
using Newtonsoft.Json;

namespace PersonalCapital.Extensions;

public class NanToNullConverter : JsonConverter<decimal?>
{
    public override void WriteJson(JsonWriter writer, decimal? value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }

    public override decimal? ReadJson(JsonReader reader, Type objectType, decimal? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String &&
            reader.Value is string value &&
            value.Equals("nan", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return serializer.Deserialize<decimal?>(reader);
    }
}