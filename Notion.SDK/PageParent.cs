using System;

namespace Notion
{
    public abstract record Parent
    {
        public string Type { get; set; } = null!;
    }

    public record DatabaseParent : Parent
    {
        public Guid DatabaseId { get; set; }
    }

    public record PageParent : Parent
    {
        public Guid PageId { get; set; }
    }

    public record WorkspaceParent : Parent
    {
    }
}