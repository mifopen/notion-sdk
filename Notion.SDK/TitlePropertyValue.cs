namespace Notion
{
    public record TitlePropertyValue : IPropertyValue
    {
        public string Id { get; set; }
        public PropertyValueType Type { get; set; }

        public IRichText[] Text { get; set; }
    }
}