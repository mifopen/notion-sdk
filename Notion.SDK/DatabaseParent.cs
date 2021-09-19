using System;

namespace Notion
{
    public class DatabaseParent : Parent
    {
        public DatabaseParent()
        {
            Type = ParentType.Database;
        }

        public Guid DatabaseId { get; set; }
    }
}