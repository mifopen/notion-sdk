namespace Notion
{
    public record Paragraph : Block
    {
        public RichText[] Text { get; set; }
    }
}