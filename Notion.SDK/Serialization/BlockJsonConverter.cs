using System;
using System.Text.Json;

namespace Notion.Serialization
{
    internal class BlockJsonConverter : PolymorphicJsonConverterBase<Block>
    {
        public override Block Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var type = Helpers.ReadProperty(ref reader, Constants.BlockProperty.Type);

            Block block = type switch
            {
                Constants.BlockType.Paragraph => new Paragraph(),
                Constants.BlockType.Heading1 => new Heading1(),
                Constants.BlockType.Heading2 => new Heading2(),
                Constants.BlockType.Heading3 => new Heading3(),
                Constants.BlockType.BulletedListItem => new BulletedListItem(),
                Constants.BlockType.Unsupported => new Unsupported(),
                _ => new Unsupported(),
            };

            Helpers.ReadObject(ref reader, block, ReadBlockProperty);

            return block;
        }

        private static void ReadBlockProperty(ref Utf8JsonReader reader, Block block, string propertyName)
        {
            switch (propertyName)
            {
                case Constants.BlockProperty.Id:
                    block.Id = reader.GetGuid();
                    break;
                case Constants.BlockProperty.Object:
                    reader.Skip();
                    break;
                case Constants.BlockProperty.CreatedTime:
                    block.CreatedTime = reader.GetDateTime();
                    break;
                case Constants.BlockProperty.LastEditedTime:
                    block.LastEditedTime = reader.GetDateTime();
                    break;
                case Constants.BlockProperty.Archived:
                    block.Archived = reader.GetBoolean();
                    break;
                case Constants.BlockProperty.HasChildren:
                    block.HasChildren = reader.GetBoolean();
                    break;
                case Constants.BlockProperty.Type:
                    reader.Skip();
                    break;
                case Constants.BlockType.Paragraph:
                    Helpers.ReadObject(ref reader, (Paragraph)block, ReadParagraphProperty);
                    break;
                case Constants.BlockType.Heading1:
                    Helpers.ReadObject(ref reader, (Heading1)block, ReadHeading1Property);
                    break;
                case Constants.BlockType.Heading2:
                    Helpers.ReadObject(ref reader, (Heading2)block, ReadHeading2Property);
                    break;
                case Constants.BlockType.Heading3:
                    Helpers.ReadObject(ref reader, (Heading3)block, ReadHeading3Property);
                    break;
                case Constants.BlockType.BulletedListItem:
                    Helpers.ReadObject(ref reader, (BulletedListItem)block, ReadBulletedListItemProperty);
                    break;
                //todo
                default:
                    reader.Skip();
                    break;
            }
        }

        private static void ReadParagraphProperty(ref Utf8JsonReader reader, Paragraph paragraph, string propertyName)
        {
            switch (propertyName)
            {
                case Constants.ParagraphProperty.Text:
                    paragraph.Text = JsonSerializer.Deserialize<RichText[]>(ref reader);
                    break;
                case Constants.ParagraphProperty.Children:
                    reader.Skip();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName, null);
            }
        }

        private static void ReadHeading1Property(ref Utf8JsonReader reader, Heading1 heading1, string propertyName)
        {
            switch (propertyName)
            {
                case Constants.Heading1Property.Text:
                    heading1.Text = JsonSerializer.Deserialize<RichText[]>(ref reader);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName, null);
            }
        }

        private static void ReadHeading2Property(ref Utf8JsonReader reader, Heading2 heading2, string propertyName)
        {
            switch (propertyName)
            {
                case Constants.Heading2Property.Text:
                    heading2.Text = JsonSerializer.Deserialize<RichText[]>(ref reader);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName, null);
            }
        }

        private static void ReadHeading3Property(ref Utf8JsonReader reader, Heading3 heading3, string propertyName)
        {
            switch (propertyName)
            {
                case Constants.Heading3Property.Text:
                    heading3.Text = JsonSerializer.Deserialize<RichText[]>(ref reader);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName, null);
            }
        }

        private static void ReadBulletedListItemProperty(ref Utf8JsonReader reader, BulletedListItem item,
            string propertyName)
        {
            switch (propertyName)
            {
                case Constants.BulletedListItemProperty.Text:
                    item.Text = JsonSerializer.Deserialize<RichText[]>(ref reader);
                    break;
                case Constants.BulletedListItemProperty.Children:
                    reader.Skip();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName, null);
            }
        }

        public override void Write(Utf8JsonWriter writer, Block block, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(Constants.BlockProperty.Id, block.Id);
            writer.WriteString(Constants.BlockProperty.Object, block.Object);
            writer.WriteString(Constants.BlockProperty.CreatedTime, block.CreatedTime);
            writer.WriteString(Constants.BlockProperty.LastEditedTime, block.LastEditedTime);
            writer.WriteBoolean(Constants.BlockProperty.Archived, block.Archived);
            writer.WriteBoolean(Constants.BlockProperty.HasChildren, block.HasChildren);

            var type = BlockTypeToString(block.Type);
            writer.WriteString(Constants.BlockProperty.Type, type);
            writer.WritePropertyName(type);
            writer.WriteStartObject();

            switch (block)
            {
                case Heading1 heading1:
                    writer.WritePropertyName(Constants.Heading1Property.Text);
                    JsonSerializer.Serialize(writer, heading1.Text, options);
                    break;
                case Heading2 heading2:
                    writer.WritePropertyName(Constants.Heading2Property.Text);
                    JsonSerializer.Serialize(writer, heading2.Text, options);
                    break;
                case Heading3 heading3:
                    writer.WritePropertyName(Constants.Heading3Property.Text);
                    JsonSerializer.Serialize(writer, heading3.Text, options);
                    break;
                case Paragraph paragraph:
                    writer.WritePropertyName(Constants.ParagraphProperty.Text);
                    JsonSerializer.Serialize(writer, paragraph.Text, options);
                    break;
                case BulletedListItem bulletedListItem:
                    writer.WritePropertyName(Constants.BulletedListItemProperty.Text);
                    JsonSerializer.Serialize(writer, bulletedListItem.Text, options);
                    break;
                case Unsupported:
                    //todo
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(block));
            }

            writer.WriteEndObject();

            writer.WriteEndObject();
        }

        private static string BlockTypeToString(BlockType blockType)
        {
            return blockType switch
            {
                BlockType.Unsupported => Constants.BlockType.Unsupported,
                BlockType.Heading1 => Constants.BlockType.Heading1,
                BlockType.Heading2 => Constants.BlockType.Heading2,
                BlockType.Heading3 => Constants.BlockType.Heading3,
                BlockType.Paragraph => Constants.BlockType.Paragraph,
                BlockType.BulletedListItem => Constants.BlockType.BulletedListItem,
                _ => throw new ArgumentOutOfRangeException(nameof(blockType), blockType, null)
            };
        }
    }
}