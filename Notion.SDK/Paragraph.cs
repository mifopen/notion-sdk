namespace Notion
{
    public class Paragraph : Block
    {
        public override BlockType Type => BlockType.Paragraph;
        public RichText[] Text { get; set; }
    }
}