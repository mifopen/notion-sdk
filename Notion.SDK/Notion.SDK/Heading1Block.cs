namespace Notion
{
    public record Heading1Block : Block
    {
        public RichTextObject[] Text { get; set; }
    }
}