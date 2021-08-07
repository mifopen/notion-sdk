using System;

namespace Notion
{
    public record DateMention : Mention
    {
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
    }
}