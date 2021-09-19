using System;
using System.Text.Json.Serialization;

namespace Notion.Serialization
{
    internal abstract class PolymorphicJsonConverterBase<T> : JsonConverter<T>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(T).IsAssignableFrom(typeToConvert);
        }
    }
}