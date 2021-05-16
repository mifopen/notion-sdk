using System;
using System.Text.Json.Serialization;

namespace Notion
{
    [JsonConverter(typeof(BlockJsonConverter))]
    public abstract record Block
    {
        public Guid Id { get; set; }
        public string Object { get; set; }
        public string Type { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastEditedTime { get; set; }
        public bool HasChildren { get; set; }
    }
}