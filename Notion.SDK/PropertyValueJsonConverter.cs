using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Notion
{
    public class PropertyValueJsonConverter : JsonConverter<PropertyValue>
    {
        public override PropertyValue? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var id = jsonDocument.RootElement.GetProperty("id").GetString();
            var type = jsonDocument.RootElement.GetProperty("type").GetString()!;
            var value = jsonDocument.RootElement.GetProperty(type);
            return type switch
            {
                "title" => new TitlePropertyValue
                {
                    Id = id!,
                    Type = type!,
                    Text = value.EnumerateArray().Select(RichTextJsonConverter.Convert).ToArray(),
                },
                _ => throw new NotSupportedException($"Property value with type {type} is not supported"),
            };
        }

        public override void Write(Utf8JsonWriter writer, PropertyValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}