using System;
using System.Text.Json.Serialization;

namespace Notion
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class JsonInterfaceConverterAttribute : JsonConverterAttribute
    {
        public JsonInterfaceConverterAttribute(Type converterType)
            : base(converterType)
        {
        }
    }
}