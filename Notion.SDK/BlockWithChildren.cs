namespace Notion
{
    public abstract record BlockWithChildren : Block, IWithChildren
    {
        public Block[]? Children { get; set; }
        public bool HasChildren { get; set; }
    }
}