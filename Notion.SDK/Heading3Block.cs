namespace Notion
{
    public record Heading3Block : Block
    {
        public RichTextObject[] Text { get; set; }
    }
}