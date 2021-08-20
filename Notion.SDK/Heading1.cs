using System;

namespace Notion
{
    public record Heading1 : IBlock
    {
        public Guid Id { get; set; }
        public string Object { get; set; }
        public string Type { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastEditedTime { get; set; }
        public IRichText[] Text { get; set; }
    }
}