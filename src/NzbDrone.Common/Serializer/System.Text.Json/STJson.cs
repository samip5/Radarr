using System.Data;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NzbDrone.Common.Serializer
{
    public static class STJson
    {
        private static readonly JsonSerializerOptions SerializerSettings = GetSerializerSettings();
        private static readonly JsonWriterOptions WriterOptions;

        static STJson()
        {
            // SerializerSettings = GetSerializerSettings();
            WriterOptions = new JsonWriterOptions
            {
                Indented = true
            };
        }

        public static JsonSerializerOptions GetSerializerSettings()
        {
            var serializerSettings = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            serializerSettings.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            serializerSettings.Converters.Add(new STJVersionConverter());
            serializerSettings.Converters.Add(new STJHttpUriConverter());
            serializerSettings.Converters.Add(new STJTimeSpanConverter());
            serializerSettings.Converters.Add(new STJUtcConverter());

            return serializerSettings;
        }

        public static string ToSTJson(this object obj)
        {
            return JsonSerializer.Serialize(obj, SerializerSettings);
        }

        public static void Serialize<TModel>(TModel model, Stream outputStream, JsonSerializerOptions options = null)
        {
            if (options == null)
            {
                options = SerializerSettings;
            }

            // Cast to object to get all properties written out
            // https://github.com/dotnet/corefx/issues/38650
            using (var writer = new Utf8JsonWriter(outputStream, options: WriterOptions))
            {
                JsonSerializer.Serialize(writer, (object)model, options);
            }
        }
    }
}
