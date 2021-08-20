namespace Notion
{
    public interface IWithChildren
    {
        public Block[]? Children { get; set; }
        public bool HasChildren { get; }
    }
}