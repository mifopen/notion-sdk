using System;

namespace Notion
{
    public record Unsupported : IBlock
    {
        public Guid Id { get; set; }
        public string Object { get; set; }
        public string Type { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastEditedTime { get; set; }
    }
}