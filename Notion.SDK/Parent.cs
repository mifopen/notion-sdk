using System.Text.Json.Serialization;
using Notion.Serialization;

namespace Notion
{
    [JsonConverter(typeof(ParentJsonConverter))]
    public abstract class Parent
    {
        public ParentType Type { get; set; }
    }
}