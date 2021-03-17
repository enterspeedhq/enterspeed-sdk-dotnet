#if NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace Enterspeed.Source.Sdk.Domain.SystemTextJson
{
    public class EnterspeedPropertyConverter : JsonConverter<IEnterspeedProperty>
    {
        public override IEnterspeedProperty Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException("Cannot deserialize an IEnterspeedProperty");
        }

        public override void Write(Utf8JsonWriter writer, IEnterspeedProperty value, JsonSerializerOptions options)
        {
            WriteProperties(writer, value, options);
        }

        private static void WriteProperties(Utf8JsonWriter writer, IEnterspeedProperty value, JsonSerializerOptions options, bool createObject = true)
        {
            if (createObject)
            {
                writer.WriteStartObject();
            }

            var type = value.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(value);
                var jsonEncodedPropertyName = GetPropertyName(property.Name);

                switch (propertyValue)
                {
                    case string stringProperty:
                        writer.WriteString(jsonEncodedPropertyName, stringProperty);
                        break;

                    case double doublePropertyValue:
                        writer.WriteString(jsonEncodedPropertyName, doublePropertyValue.ToString(CultureInfo.InvariantCulture));
                        break;

                    case bool boolPropertyValue:
                        writer.WriteBoolean(jsonEncodedPropertyName, boolPropertyValue);
                        break;

                    case IEnumerable<IEnterspeedProperty> arrayProperty:
                        writer.WriteStartArray(jsonEncodedPropertyName);

                        foreach (var enterspeedProperty in arrayProperty)
                        {
                            WriteProperties(writer, enterspeedProperty, options);
                        }

                        writer.WriteEndArray();
                        break;

                    case IDictionary<string, IEnterspeedProperty> objectProperties:
                        writer.WriteStartObject(jsonEncodedPropertyName);

                        foreach (var objectProperty in objectProperties)
                        {
                            writer.WriteStartObject(GetPropertyName(objectProperty.Key));
                            WriteProperties(writer, objectProperty.Value, options, false);
                            writer.WriteEndObject();
                        }

                        writer.WriteEndObject();
                        break;
                }
            }

            if (createObject)
            {
                writer.WriteEndObject();
            }
        }

        private static JsonEncodedText GetPropertyName(string propertyName)
        {
            return JsonEncodedText.Encode(propertyName);
        }
    }
}
#endif