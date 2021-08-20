namespace Notion
{
    public interface IMention : IRichText
    {
        public MentionType MentionType { get; }
    }
}