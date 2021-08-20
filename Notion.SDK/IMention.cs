namespace Notion
{
    [JsonInterfaceConverter(typeof(JsonConverters.PolymorphicConverter<IMention>))]
    public interface IMention : IRichText
    {
        public MentionType MentionType { get; }
    }
}