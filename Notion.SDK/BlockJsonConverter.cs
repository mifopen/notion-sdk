using System;
using System.Linq;
using System.Text.Json;

namespace Notion
{
    public static class BlockJsonConverter
    {
        public static Block Convert(JsonDocument jsonDocument)
        {
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
                    Text = value.GetProperty("text").EnumerateArray().Select(RichTextJsonConverter.Convert).ToArray(),
                },
                "heading_2" => new Heading2
                {
                    Id = id,
                    Type = type,
                    Object = @object,
                    CreatedTime = createdTime,
                    LastEditedTime = lastEditedTime,
                    Text = value.GetProperty("text").EnumerateArray().Select(RichTextJsonConverter.Convert).ToArray(),
                },
                "heading_3" => new Heading3
                {
                    Id = id,
                    Type = type,
                    Object = @object,
                    CreatedTime = createdTime,
                    LastEditedTime = lastEditedTime,
                    Text = value.GetProperty("text").EnumerateArray().Select(RichTextJsonConverter.Convert).ToArray(),
                },
                "bulleted_list_item" => new BulletedListItem
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
    }
}