namespace Notion
{
    public record Paragraph : BlockWithChildren
    {
        public RichText[] Text { get; set; }
    }
}