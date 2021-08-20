using System;

namespace Notion
{
    public record DatabaseParent : IParent
    {
        public ParentType Type { get; set; }
        public Guid DatabaseId { get; set; }
    }
}