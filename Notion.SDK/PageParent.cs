using System;

namespace Notion
{
    public class PageParent : Parent
    {
        public PageParent()
        {
            Type = ParentType.Page;
        }

        public Guid PageId { get; set; }
    }
}