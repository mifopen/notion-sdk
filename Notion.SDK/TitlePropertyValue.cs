namespace Notion
{
    public record TitlePropertyValue : PropertyValue
    {
        public RichTextObject[] Text { get; set; }
    }
}