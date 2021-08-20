namespace Notion
{
    public interface IPropertyValue
    {
        public string Id { get; }
        public PropertyValueType Type { get; }
    }

    public enum PropertyValueType
    {
        Title,
    }
}