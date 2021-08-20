namespace Notion
{
    [JsonInterfaceConverter(typeof(JsonConverters.PolymorphicConverter<IWithChildren>))]
    public interface IWithChildren
    {
        public IBlock[]? Children { get; set; }
        public bool HasChildren { get; }
    }
}