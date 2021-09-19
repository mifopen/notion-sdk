using System;

namespace Notion
{
    public class DateMention : Mention
    {
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
    }
}