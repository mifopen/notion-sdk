namespace Notion
{
    public record BulletedListItem : Block
    {
        public RichText[] Text { get; set; }
    }
}