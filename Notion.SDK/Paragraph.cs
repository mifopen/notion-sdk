using System;

namespace Notion
{
    public record Paragraph : IBlock, IWithChildren
    {
        public Guid Id { get; set; }
        public string Object { get; set; }
        public string Type { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastEditedTime { get; set; }
        public IBlock[]? Children { get; set; }
        public bool HasChildren { get; set; }
        public IRichText[] Text { get; set; }
    }
}