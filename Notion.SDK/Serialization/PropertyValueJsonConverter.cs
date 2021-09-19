using System;
using System.Text.Json;

namespace Notion.Serialization
{
    internal class PropertyValueJsonConverter : PolymorphicJsonConverterBase<PropertyValue>
    {
        public override PropertyValue Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var type = Helpers.ReadProperty(ref reader, Constants.PropertyValueProperty.Type);

            PropertyValue propertyValue = type switch
            {
                Constants.PropertyValueType.Title => new TitlePropertyValue(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };

            Helpers.ReadObject(ref reader, propertyValue, ReadPropertyValueProperty);

            return propertyValue;
        }

        private static void ReadPropertyValueProperty(ref Utf8JsonReader reader, PropertyValue propertyValue,
            string propertyName)
        {
            switch (propertyName)
            {
                case Constants.PropertyValueProperty.Id:
                    propertyValue.Id = reader.GetString()!;
                    break;
                case Constants.PropertyValueProperty.Type:
                    reader.Skip();
                    break;
                case Constants.PropertyValueType.Title:
                    ((TitlePropertyValue)propertyValue).Text = JsonSerializer.Deserialize<RichText[]>(ref reader);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName, null);
            }
        }

        public override void Write(Utf8JsonWriter writer, PropertyValue value, JsonSerializerOptions options)
        {
            //todo
            throw new NotImplementedException();
        }
    }
}