using System;

namespace Notion
{
    public record Heading2 : IBlock
    {
        public IRichText[] Text { get; set; }
        public Guid Id { get; set; }
        public string Object { get; set; }
        public string Type { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastEditedTime { get; set; }
    }
}