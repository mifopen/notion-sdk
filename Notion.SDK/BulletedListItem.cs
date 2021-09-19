namespace Notion
{
    public class BulletedListItem : Block
    {
        public override BlockType Type => BlockType.BulletedListItem;
        public RichText[] Text { get; set; }
    }
}