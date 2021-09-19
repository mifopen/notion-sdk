using System;
using System.Text.Json;

namespace Notion.Serialization
{
    internal class PageJsonConverter : PolymorphicJsonConverterBase<Page>
    {
        public override Page Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var page = new Page();
            Helpers.ReadObject(ref reader, page, ReadPageProperty);
            return page;
        }

        private static void ReadPageProperty(ref Utf8JsonReader reader, Page page, string propertyName)
        {
            switch (propertyName)
            {
                case Constants.PageProperty.Id:
                    page.Id = reader.GetGuid();
                    break;
                case Constants.PageProperty.Object:
                    reader.Skip();
                    break;
                case Constants.PageProperty.CreatedTime:
                    page.CreatedTime = reader.GetDateTime();
                    break;
                case Constants.PageProperty.LastEditedTime:
                    page.LastEditedTime = reader.GetDateTime();
                    break;
                case Constants.PageProperty.Archived:
                    page.Archived = reader.GetBoolean();
                    break;
                case Constants.PageProperty.Icon:
                    //todo
                    reader.Skip();
                    break;
                case Constants.PageProperty.Cover:
                    //todo
                    reader.Skip();
                    break;
                case Constants.PageProperty.Properties:
                    Helpers.ReadObject(ref reader, page, ReadPageProperties);
                    break;
                case Constants.PageProperty.Parent:
                    page.Parent = JsonSerializer.Deserialize<Parent>(ref reader);
                    break;
                case Constants.PageProperty.Url:
                    page.Url = reader.GetString()!;
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        private static void ReadPageProperties(ref Utf8JsonReader reader, Page page, string propertyName)
        {
            page.Properties.Add(propertyName, JsonSerializer.Deserialize<PropertyValue>(ref reader));
        }

        public override void Write(Utf8JsonWriter writer, Page page, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(Constants.PageProperty.Id, page.Id);
            writer.WriteString(Constants.PageProperty.Object, page.Object);
            writer.WriteString(Constants.PageProperty.CreatedTime, page.CreatedTime);
            writer.WriteString(Constants.PageProperty.LastEditedTime, page.LastEditedTime);
            writer.WriteBoolean(Constants.PageProperty.Archived, page.Archived);
            writer.WritePropertyName(Constants.PageProperty.Parent);
            JsonSerializer.Serialize(writer, page.Parent, options);
            writer.WriteString(Constants.PageProperty.Url, page.Url);
            // todo 
            // writer.WriteBoolean(Constants.PageProperty.Icon, page.Archived);
            // writer.WriteBoolean(Constants.PageProperty.Cover, page.Archived);
            // writer.WriteBoolean(Constants.PageProperty.Properties, page.Archived);

            writer.WriteEndObject();
        }
    }
}