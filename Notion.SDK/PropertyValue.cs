namespace Notion
{
    public abstract record PropertyValue
    {
        public string Id { get; set; }
        public string Type { get; set; }
    }
}