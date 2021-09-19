namespace Notion
{
    public class TitlePropertyValue : PropertyValue
    {
        public TitlePropertyValue()
        {
            Type = PropertyValueType.Title;
        }

        public RichText[] Text { get; set; }
    }
}