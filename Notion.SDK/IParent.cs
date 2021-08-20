namespace Notion
{
    [JsonInterfaceConverter(typeof(JsonConverters.PolymorphicConverter<IParent>))]
    public interface IParent
    {
        public ParentType Type { get; }
    }
}