using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Notion.Serialization;

namespace Notion
{
    [JsonConverter(typeof(PageJsonConverter))]
    public class Page
    {
        public Guid Id { get; set; }
        public string Object => "page";
        public DateTime CreatedTime { get; set; }
        public DateTime LastEditedTime { get; set; }
        public Block[]? Children { get; set; }
        public bool HasChildren { get; set; }
        public bool Archived { get; set; }
        public Parent Parent { get; set; } = null!;
        public Dictionary<string, PropertyValue> Properties { get; } = new();
        public string Url { get; set; }
    }
}