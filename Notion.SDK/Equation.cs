namespace Notion
{
    public record Equation : RichText
    {
        public string Expression { get; set; }
    }
}