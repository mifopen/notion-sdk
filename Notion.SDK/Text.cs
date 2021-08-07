namespace Notion
{
    public record Text : RichText
    {
        public string Content { get; set; }
        public Link? Link { get; set; }
    }
}