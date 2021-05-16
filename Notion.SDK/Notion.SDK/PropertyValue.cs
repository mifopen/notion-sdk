using System.Text.Json.Serialization;

namespace Notion
{
    [JsonConverter(typeof(PropertyValueJsonConverter))]
    public abstract record PropertyValue
    {
        public string Id { get; set; }
        public string Type { get; set; }
    }
}