using System;

namespace Notion
{
    public record PageParent : IParent
    {
        public ParentType Type { get; set; }
        public Guid PageId { get; set; }
    }
}