namespace Notion
{
    public record BulletedListItem : BlockWithChildren
    {
        public RichText[] Text { get; set; }
    }
}