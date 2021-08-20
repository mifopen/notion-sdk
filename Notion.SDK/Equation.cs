using System.Text.Json.Serialization;

namespace Notion
{
    public record Equation : IRichText
    {
        public string PlainText { get; set; }
        public string? Href { get; set; }
        public RichTextAnnotations Annotations { get; set; }
        public RichTextType Type { get; set; }

        public string Expression { get; set; }
    }
}