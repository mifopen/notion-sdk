using System;

namespace Notion
{
    public record PageMention : Mention
    {
        public Guid Id { get; set; }
    }
}