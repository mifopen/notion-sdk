using System;
using System.Text.Json.Serialization;

namespace Notion
{
    public record DatabaseMention : IMention
    {
        public string PlainText { get; set; }
        public string? Href { get; set; }
        public RichTextAnnotations Annotations { get; set; }
        public RichTextType Type { get; set; }

        public MentionType MentionType { get; set; }

        public Guid DatabaseId { get; set; }
    }
}