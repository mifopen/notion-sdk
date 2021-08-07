namespace Notion
{
    public record Heading3 : Block
    {
        public RichText[] Text { get; set; }
    }
}