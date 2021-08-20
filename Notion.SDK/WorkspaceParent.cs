namespace Notion
{
    public record WorkspaceParent : IParent
    {
        public ParentType Type { get; set; }
    }
}