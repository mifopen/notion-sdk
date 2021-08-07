namespace Notion
{
    public record Heading2 : Block
    {
        public RichText[] Text { get; set; }
    }
}