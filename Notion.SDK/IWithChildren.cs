namespace Notion
{
    public interface IWithChildren
    {
        public IBlock[]? Children { get; set; }
        public bool HasChildren { get; }
    }
}