using System.Text.Json.Serialization;

namespace Notion
{
    public record Text : IRichText
    {
        public string PlainText { get; set; }
        public string? Href { get; set; }
        public RichTextAnnotations Annotations { get; set; }
        public RichTextType Type { get; set; }

        public Link? Link { get; set; }
        public string Content { get; set; }
    }
}