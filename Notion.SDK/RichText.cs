using System.Text.Json.Serialization;
using Notion.Serialization;

namespace Notion
{
    [JsonConverter(typeof(RichTextJsonConverter))]
    public abstract class RichText
    {
        public string PlainText { get; set; }
        public string? Href { get; set; }
        public RichTextAnnotations Annotations { get; set; }
        public RichTextType Type { get; set; }
    }
}