using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Notion
{
    public static class JsonConverters
    {
        public static IBlock ConvertBlock(JsonElement json)
        {
            var id = json.GetProperty("id").GetGuid();
            var type = json.GetProperty("type").GetString()!;
            var @object = json.GetProperty("object").GetString()!;
            var createdTime = json.GetProperty("created_time").GetDateTime();
            var lastEditedTime = json.GetProperty("last_edited_time").GetDateTime();
            var hasChildren = json.GetProperty("has_children").GetBoolean();
            var value = json.GetProperty(type);
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
                    Text = value.GetProperty("text").EnumerateArray().Select(ConvertRichText).ToArray(),
                },
                "heading_1" => new Heading1
                {
                    Id = id,
                    Type = type,
                    Object = @object,
                    CreatedTime = createdTime,
                    LastEditedTime = lastEditedTime,
                    Text = value.GetProperty("text").EnumerateArray().Select(ConvertRichText).ToArray(),
                },
                "heading_2" => new Heading2
                {
                    Id = id,
                    Type = type,
                    Object = @object,
                    CreatedTime = createdTime,
                    LastEditedTime = lastEditedTime,
                    Text = value.GetProperty("text").EnumerateArray().Select(ConvertRichText).ToArray(),
                },
                "heading_3" => new Heading3
                {
                    Id = id,
                    Type = type,
                    Object = @object,
                    CreatedTime = createdTime,
                    LastEditedTime = lastEditedTime,
                    Text = value.GetProperty("text").EnumerateArray().Select(ConvertRichText).ToArray(),
                },
                "bulleted_list_item" => new BulletedListItem
                {
                    Id = id,
                    Type = type,
                    Object = @object,
                    CreatedTime = createdTime,
                    LastEditedTime = lastEditedTime,
                    HasChildren = hasChildren,
                    Text = value.GetProperty("text").EnumerateArray().Select(ConvertRichText).ToArray(),
                },
                "unsupported" => new Unsupported(),
                _ => throw new NotSupportedException($"Block with type {type} is not supported"),
            };
        }

        public static Page ConvertPage(JsonElement json)
        {
            return new Page
            {
                Id = json.GetProperty("id").GetGuid(),
                Object = json.GetProperty("object").GetString()!,
                CreatedTime = json.GetProperty("created_time").GetDateTime(),
                LastEditedTime = json.GetProperty("last_edited_time").GetDateTime(),
                Archived = json.GetProperty("archived").GetBoolean(),
                Children = null,
                Parent = ConvertParent(json.GetProperty("parent")),
                Properties = ConvertProperties(json.GetProperty("properties")),
                Url = json.GetProperty("url").GetString()!,
            };
        }

        private static IParent ConvertParent(JsonElement json)
        {
            var type = json.GetProperty("type").GetString()!;
            return type switch
            {
                "page_id" => new PageParent
                {
                    Type = ParentType.Page,
                    PageId = json.GetProperty("page_id").GetGuid(),
                },
                "database_id" => new DatabaseParent
                {
                    Type = ParentType.Database,
                    DatabaseId = json.GetProperty("database_id").GetGuid(),
                },
                "workspace" => new WorkspaceParent
                {
                    Type = ParentType.Workspace,
                },
                _ => throw new NotSupportedException($"Parent with type {type} is not supported"),
            };
        }

        private static Dictionary<string, IPropertyValue> ConvertProperties(JsonElement json)
        {
            return json.EnumerateObject()
                .ToDictionary(x => x.Name,
                    x => ConvertPropertyValue(x.Value));
        }

        private static IPropertyValue ConvertPropertyValue(JsonElement json)
        {
            var id = json.GetProperty("id").GetString()!;
            var type = json.GetProperty("type").GetString()!;
            var value = json.GetProperty(type);
            return type switch
            {
                "title" => new TitlePropertyValue
                {
                    Id = id,
                    Type = PropertyValueType.Title,
                    Text = value.EnumerateArray().Select(ConvertRichText).ToArray(),
                },
                _ => throw new NotSupportedException($"Property value with type {type} is not supported"),
            };
        }

        private static IRichText ConvertRichText(JsonElement element)
        {
            var type = element.GetProperty("type").GetString()!;
            var value = element.GetProperty(type);
            var annotations = element.GetProperty("annotations");
            return type switch
            {
                "text" => new Text
                {
                    Type = RichTextType.Text,
                    PlainText = element.GetProperty("plain_text").GetString()!,
                    Link = value.TryGetProperty("link", out var link)
                           && link.ValueKind != JsonValueKind.Null
                        ? new Link
                        {
                            Type = "url",
                            Url = link.GetProperty("url").GetString()!,
                        }
                        : null,
                    Annotations = new RichTextAnnotations
                    {
                        Bold = annotations.GetProperty("bold").GetBoolean(),
                        Italic = annotations.GetProperty("italic").GetBoolean(),
                        Strikethrough = annotations.GetProperty("strikethrough").GetBoolean(),
                        Underline = annotations.GetProperty("underline").GetBoolean(),
                        Code = annotations.GetProperty("code").GetBoolean(),
                        Color = annotations.GetProperty("color").GetString()!,
                    },
                    Content = value.GetProperty("content").GetString()!,
                    Href = element.TryGetProperty("href", out var href)
                           && href.ValueKind != JsonValueKind.Null
                        ? href.GetString()
                        : null,
                },
                _ => throw new NotSupportedException($"Rich text with type {type} is not supported"),
            };
        }

        public class PolymorphicConverter<T> : JsonConverter<T>
        {
            public override T Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(
                Utf8JsonWriter writer,
                T? value,
                JsonSerializerOptions options)
            {
                switch (value)
                {
                    case null:
                        writer.WriteNullValue();
                        break;
                    default:
                    {
                        var type = value.GetType();
                        JsonSerializer.Serialize(writer, value, type, options);
                        break;
                    }
                }
            }
        }
    }
}