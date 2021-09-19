using System;
using System.Text.Json.Serialization;
using Notion.Serialization;

namespace Notion
{
    [JsonConverter(typeof(BlockJsonConverter))]
    public abstract class Block
    {
        public Guid Id { get; set; }
        public string Object => "block";
        public abstract BlockType Type { get; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastEditedTime { get; set; }
        public bool Archived { get; set; }
        public bool HasChildren { get; set; }
        public Block[]? Children { get; set; }
    }
}