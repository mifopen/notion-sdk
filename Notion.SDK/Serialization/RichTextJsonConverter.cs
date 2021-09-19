using System;
using System.Text.Json;

namespace Notion.Serialization
{
    internal class RichTextJsonConverter : PolymorphicJsonConverterBase<RichText>
    {
        public override RichText Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var type = Helpers.ReadProperty(ref reader, Constants.RichTextProperty.Type);

            RichText richText = type switch
            {
                Constants.RichTextType.Text => new Text(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };

            Helpers.ReadObject(ref reader, richText, ReadBlockProperty);

            return richText;
        }

        private static void ReadBlockProperty(ref Utf8JsonReader reader, RichText richText, string propertyName)
        {
            switch (propertyName)
            {
                case Constants.RichTextProperty.PlainText:
                    richText.PlainText = reader.GetString()!;
                    break;
                case Constants.RichTextProperty.Href:
                    richText.Href = reader.GetString();
                    break;
                case Constants.RichTextProperty.Annotations:
                    var annotations = new RichTextAnnotations();
                    Helpers.ReadObject(ref reader, annotations, ReadAnnotationsProperty);
                    richText.Annotations = annotations;
                    break;
                case Constants.RichTextProperty.Type:
                    reader.Skip();
                    break;
                case Constants.RichTextType.Text:
                    Helpers.ReadObject(ref reader, (Text)richText, ReadTextProperty);
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        private static void ReadTextProperty(ref Utf8JsonReader reader, Text text, string propertyName)
        {
            switch (propertyName)
            {
                case Constants.TextProperty.Content:
                    text.Content = reader.GetString()!;
                    break;
                case Constants.TextProperty.Link:
                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        reader.Skip();
                    }
                    else
                    {
                        Helpers.ReadObject(ref reader, text, ReadTextLinkProperty);
                    }

                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        private static void ReadTextLinkProperty(ref Utf8JsonReader reader, Text text, string propertyName)
        {
            switch (propertyName)
            {
                case "url":
                    text.Link = reader.GetString();
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        private static void ReadAnnotationsProperty(ref Utf8JsonReader reader, RichTextAnnotations annotations,
            string propertyName)
        {
            switch (propertyName)
            {
                case "bold":
                    annotations.Bold = reader.GetBoolean();
                    break;
                case "italic":
                    annotations.Italic = reader.GetBoolean();
                    break;
                case "color":
                    annotations.Color = reader.GetString()!;
                    break;
                //todo
                default:
                    reader.Skip();
                    break;
            }
        }

        public override void Write(Utf8JsonWriter writer, RichText richText, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("plain_text", richText.PlainText);
            if (richText.Href != null)
            {
                writer.WriteString("href", richText.Href);
            }

            writer.WritePropertyName("annotations");
            writer.WriteStartObject();
            writer.WriteBoolean("bold", richText.Annotations.Bold);
            writer.WriteBoolean("italic", richText.Annotations.Italic);
            writer.WriteBoolean("strikethrough", richText.Annotations.Strikethrough);
            writer.WriteBoolean("underline", richText.Annotations.Underline);
            writer.WriteBoolean("code", richText.Annotations.Code);
            writer.WriteString("color", richText.Annotations.Color);
            writer.WriteEndObject();

            var type = RichTextTypeToString(richText.Type);
            writer.WriteString("type", type);
            writer.WritePropertyName(type);
            writer.WriteStartObject();
            switch (richText)
            {
                case Text text:
                    writer.WriteString("content", text.Content);
                    if (text.Link != null)
                    {
                        WriteLink(writer, text.Link);
                    }

                    break;
                case Equation equation:
                    writer.WriteString("expression", equation.Expression);
                    break;
                case Mention mention:
                    var mentionType = MentionTypeToString(mention.MentionType);
                    writer.WriteString("type", mentionType);
                    writer.WritePropertyName(mentionType);
                    writer.WriteStartObject();

                    switch (mention)
                    {
                        case DatabaseMention databaseMention:
                            writer.WriteString("id", databaseMention.DatabaseId);
                            break;
                        case DateMention dateMention:
                            break;
                        case PageMention pageMention:
                            writer.WriteString("id", pageMention.PageId);
                            break;
                        case UserMention userMention:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(mention));
                    }

                    writer.WriteEndObject();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(richText), richText, null);
            }

            writer.WriteEndObject();

            writer.WriteEndObject();
        }

        private static void WriteLink(Utf8JsonWriter writer, string url)
        {
            writer.WriteStartObject();
            writer.WriteString("type", "url");
            writer.WriteString("url", url);
            writer.WriteEndObject();
        }

        private static string RichTextTypeToString(RichTextType richTextType)
        {
            return richTextType switch
            {
                RichTextType.Equation => "equation",
                RichTextType.Mention => "mention",
                RichTextType.Text => "text",
                _ => throw new ArgumentOutOfRangeException(nameof(richTextType), richTextType, null)
            };
        }

        private static string MentionTypeToString(MentionType mentionType)
        {
            return mentionType switch
            {
                MentionType.Database => "database",
                MentionType.Date => "date",
                MentionType.Page => "page",
                MentionType.User => "user",
                _ => throw new ArgumentOutOfRangeException(nameof(mentionType), mentionType, null),
            };
        }
    }
}