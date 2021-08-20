using System;

namespace Notion
{
    public interface IBlock
    {
        public Guid Id { get; }
        public string Object { get; }
        public string Type { get; }
        public DateTime CreatedTime { get; }
        public DateTime LastEditedTime { get; }
    }
}