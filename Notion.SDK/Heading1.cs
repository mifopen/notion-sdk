namespace Notion
{
    public class Heading1 : Block
    {
        public override BlockType Type => BlockType.Heading1;
        public RichText[] Text { get; set; }
    }
}