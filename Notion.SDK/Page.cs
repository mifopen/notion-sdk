using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Notion
{
    public record Page
    {
        public Guid Id { get; set; }
        public string Object { get; set; }
        [JsonPropertyName("created_time")] public DateTime CreatedTime { get; set; }
        [JsonPropertyName("last_edited_time")] public DateTime LastEditedTime { get; set; }
        public bool Archived { get; set; }
        public PageParent Parent { get; set; }
        public Dictionary<string, PropertyValue> Properties { get; set; }
        public string Url { get; set; }
    }
}