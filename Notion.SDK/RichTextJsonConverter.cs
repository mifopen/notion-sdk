using System;
using System.Text.Json;

namespace Notion
{
    public static class RichTextJsonConverter
    {
        public static RichText Convert(JsonElement element)
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
    }
}