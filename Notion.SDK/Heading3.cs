namespace Notion
{
    public class Heading3 : Block
    {
        public override BlockType Type => BlockType.Heading3;
        public RichText[] Text { get; set; }
    }
}