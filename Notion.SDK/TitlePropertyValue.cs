namespace Notion
{
    public record TitlePropertyValue : PropertyValue
    {
        public RichText[] Text { get; set; }
    }
}