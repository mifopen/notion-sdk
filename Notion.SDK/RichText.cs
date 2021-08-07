using System.Text.Json.Serialization;

namespace Notion
{
    public abstract record RichText
    {
        [JsonPropertyName("plain_text")] public string PlainText { get; set; }
        public string? Href { get; set; }
        public RichTextAnnotations Annotations { get; set; }
        public RichTextType Type { get; set; }
    }
}