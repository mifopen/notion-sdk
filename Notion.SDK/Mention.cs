namespace Notion
{
    public abstract record Mention : RichText
    {
        public string Type { get; set; }
    }
}