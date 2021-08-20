using System;
using System.Collections.Generic;

namespace Notion
{
    public record Page : IWithChildren
    {
        public Guid Id { get; set; }
        public string Object { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastEditedTime { get; set; }
        public bool Archived { get; set; }
        public IParent Parent { get; set; } = null!;
        public Dictionary<string, IPropertyValue> Properties { get; set; }
        public string Url { get; set; }
        public IBlock[]? Children { get; set; }
        public bool HasChildren => true;
    }
}