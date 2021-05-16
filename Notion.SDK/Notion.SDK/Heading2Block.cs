namespace Notion
{
    public record Heading2Block : Block
    {
        public RichTextObject[] Text { get; set; }
    }
}