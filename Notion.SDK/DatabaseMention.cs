using System;

namespace Notion
{
    public record DatabaseMention : Mention
    {
        public Guid Id { get; set; }
    }
}