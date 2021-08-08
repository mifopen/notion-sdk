using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Notion
{
    public class BlockJsonConverter : JsonConverter<Block>
    {
        public override Block? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var root = jsonDocument.RootElement;
            var id = root.GetProperty("id").GetGuid();
            var type = root.GetProperty("type").GetString()!;
            var @object = root.GetProperty("object").GetString()!;
            var createdTime = root.GetProperty("created_time").GetDateTime();
            var lastEditedTime = root.GetProperty("last_edited_time").GetDateTime();
            var hasChildren = root.GetProperty("has_children").GetBoolean();
            var value = root.GetProperty(type);
            return type switch
            {
                "paragraph" => new Paragraph
                {
                    Id = id,
                    Type = type,
                    Object = @object,
                    CreatedTime = createdTime,
                    LastEditedTime = lastEditedTime,
                    HasChildren = hasChildren,
                    Text = value.GetProperty("text").EnumerateArray().Select(RichTextJsonConverter.Convert).ToArray(),
                },
                "heading_1" => new Heading1
                {
                    Id = id,
                    Type = type,
                    Object = @object,
                    CreatedTime = createdTime,
                    LastEditedTime = lastEditedTime,
                    HasChildren = hasChildren,
                    Text = value.GetProperty("text").EnumerateArray().Select(RichTextJsonConverter.Convert).ToArray(),
                },
                "heading_2" => new Heading2
                {
                    Id = id,
                    Type = type,
                    Object = @object,
                    CreatedTime = createdTime,
                    LastEditedTime = lastEditedTime,
                    HasChildren = hasChildren,
                    Text = value.GetProperty("text").EnumerateArray().Select(RichTextJsonConverter.Convert).ToArray(),
                },
                "heading_3" => new Heading3
                {
                    Id = id,
                    Type = type,
                    Object = @object,
                    CreatedTime = createdTime,
                    LastEditedTime = lastEditedTime,
                    HasChildren = hasChildren,
                    Text = value.GetProperty("text").EnumerateArray().Select(RichTextJsonConverter.Convert).ToArray(),
                },
                "unsupported" => new Unsupported(),
                _ => throw new NotSupportedException($"Block with type {type} is not supported"),
            };
        }

        public override void Write(Utf8JsonWriter writer, Block value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}