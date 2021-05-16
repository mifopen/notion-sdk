namespace Notion
{
    public record ParagraphBlock : Block
    {
        public RichTextObject[] Text { get; set; }
    }
}