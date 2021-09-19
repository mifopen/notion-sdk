using System.Text.Json;

namespace Notion.Serialization
{
    internal static class Helpers
    {
        public delegate void PropertyHandler<in T>(ref Utf8JsonReader reader, T obj, string propertyName);

        public static void ReadObject<T>(ref Utf8JsonReader reader, T obj, PropertyHandler<T> action)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"Expected TokenType is StartObject, but actual is {reader.TokenType}");
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                var propertyName = reader.GetString()!;
                reader.Read();
                action(ref reader, obj, propertyName);
            }

            throw new JsonException();
        }

        public static string ReadProperty(ref Utf8JsonReader reader, string propertyName)
        {
            var typeReader = reader;
            while (typeReader.Read())
            {
                if (typeReader.TokenType == JsonTokenType.PropertyName &&
                    typeReader.ValueTextEquals(propertyName))
                {
                    if (!typeReader.Read())
                    {
                        throw new JsonException("Unable to read");
                    }

                    var typeName = typeReader.GetString();
                    if (typeName == null)
                    {
                        throw new JsonException("Unable to read the type name");
                    }

                    return typeName;
                }

                if (typeReader.TokenType is JsonTokenType.StartArray or JsonTokenType.StartObject)
                {
                    typeReader.Skip();
                }
            }

            throw new JsonException("Can't find type property");
        }
    }
}