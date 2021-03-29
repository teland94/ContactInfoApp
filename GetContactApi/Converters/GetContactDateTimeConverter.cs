using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GetContactAPI.Converters
{
    internal class GetContactDateTimeConverter : JsonConverter<DateTime>
    {
        private const string GetContactDateFormat = "yyyy-MM-dd HH:mm:ss";

        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
            DateTime.ParseExact(reader.GetString(), GetContactDateFormat, CultureInfo.InvariantCulture);

        public override void Write(
            Utf8JsonWriter writer,
            DateTime value,
            JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString(GetContactDateFormat, CultureInfo.InvariantCulture));
    }
}
