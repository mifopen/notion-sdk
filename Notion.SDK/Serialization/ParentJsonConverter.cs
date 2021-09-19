using System;
using System.Text.Json;

namespace Notion.Serialization
{
    internal class ParentJsonConverter : PolymorphicJsonConverterBase<Parent>
    {
        public override Parent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var type = Helpers.ReadProperty(ref reader, Constants.BlockProperty.Type);

            Parent parent = type switch
            {
                Constants.ParentType.Workspace => new WorkspaceParent(),
                Constants.ParentType.Database => new DatabaseParent(),
                Constants.ParentType.Page => new PageParent(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };

            Helpers.ReadObject(ref reader, parent, ReadParentProperty);

            return parent;
        }

        private static void ReadParentProperty(ref Utf8JsonReader reader, Parent parent, string propertyName)
        {
            switch (propertyName)
            {
                case Constants.ParentProperty.Type:
                    reader.Skip();
                    break;
                case Constants.ParentType.Workspace:
                    reader.Skip();
                    break;
                case Constants.ParentType.Database:
                    ((DatabaseParent)parent).DatabaseId = reader.GetGuid();
                    break;
                case Constants.ParentType.Page:
                    ((PageParent)parent).PageId = reader.GetGuid();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName, null);
            }
        }

        public override void Write(Utf8JsonWriter writer, Parent parent, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            var type = ParentTypeToString(parent.Type);
            writer.WriteString(Constants.ParentProperty.Type, type);
            writer.WritePropertyName(type);

            switch (parent)
            {
                case WorkspaceParent:
                    writer.WriteBooleanValue(true);
                    break;
                case DatabaseParent databaseParent:
                    writer.WriteStringValue(databaseParent.DatabaseId);
                    break;
                case PageParent pageParent:
                    writer.WriteStringValue(pageParent.PageId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(parent));
            }

            writer.WriteEndObject();
        }

        private static string ParentTypeToString(ParentType parentType)
        {
            return parentType switch
            {
                ParentType.Database => "database_id",
                ParentType.Page => "page_id",
                ParentType.Workspace => "workspace",
                _ => throw new ArgumentOutOfRangeException(nameof(parentType), parentType, null),
            };
        }
    }
}