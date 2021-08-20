namespace Notion
{
    [JsonInterfaceConverter(typeof(JsonConverters.PolymorphicConverter<IRichText>))]
    public interface IRichText
    {
        public string PlainText { get; }
        public string? Href { get; }
        public RichTextAnnotations Annotations { get; }
        public RichTextType Type { get; }
    }
}