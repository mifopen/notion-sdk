using System.Text.Json.Serialization;
using Notion.Serialization;

namespace Notion
{
    [JsonConverter(typeof(PropertyValueJsonConverter))]
    public abstract class PropertyValue
    {
        public string Id { get; set; }
        public PropertyValueType Type { get; set; }
    }
}