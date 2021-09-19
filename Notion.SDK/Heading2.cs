namespace Notion
{
    public class Heading2 : Block
    {
        public override BlockType Type => BlockType.Heading2;
        public RichText[] Text { get; set; }
    }
}