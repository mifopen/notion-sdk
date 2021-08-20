namespace Notion
{
    [JsonInterfaceConverter(typeof(JsonConverters.PolymorphicConverter<IPropertyValue>))]
    public interface IPropertyValue
    {
        public string Id { get; }
        public PropertyValueType Type { get; }
    }
}