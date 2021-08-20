using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Notion
{
    public static class PageJsonConverter
    {
        public static Page Convert(JsonElement json)
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

        private static Parent ConvertParent(JsonElement json)
        {
            var type = json.GetProperty("type").GetString()!;
            return type switch
            {
                "page_id" => new PageParent
                {
                    Type = type,
                    PageId = json.GetProperty("page_id").GetGuid(),
                },
                "database_id" => new DatabaseParent
                {
                    Type = type,
                    DatabaseId = json.GetProperty("database_id").GetGuid(),
                },
                "workspace" => new WorkspaceParent
                {
                    Type = type,
                },
                _ => throw new NotSupportedException($"Parent with type {type} is not supported"),
            };
        }

        private static Dictionary<string, PropertyValue> ConvertProperties(JsonElement json)
        {
            return json.EnumerateObject()
                .ToDictionary(x => x.Name,
                    x => ConvertPropertyValue(x.Value));
        }

        private static PropertyValue ConvertPropertyValue(JsonElement json)
        {
            var id = json.GetProperty("id").GetString()!;
            var type = json.GetProperty("type").GetString()!;
            var value = json.GetProperty(type);
            return type switch
            {
                "title" => new TitlePropertyValue
                {
                    Id = id,
                    Type = type,
                    Text = value.EnumerateArray().Select(RichTextJsonConverter.Convert).ToArray(),
                },
                _ => throw new NotSupportedException($"Property value with type {type} is not supported"),
            };
        }
    }
}