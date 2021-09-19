namespace Notion
{
    public abstract class Mention : RichText
    {
        public MentionType MentionType { get; set; }
    }
}