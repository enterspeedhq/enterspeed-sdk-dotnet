#if NETSTANDARD2_0 || NET6_0_OR_GREATER
using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Enterspeed.Source.Sdk.Domain.SystemTextJson
{
    public class HttpStatusCodeConverter : JsonConverter<HttpStatusCode>
    {
        public override HttpStatusCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Enum.TryParse<HttpStatusCode>(reader.GetInt32().ToString(), out var statusCode);

            return statusCode;
        }

        public override void Write(Utf8JsonWriter writer, HttpStatusCode value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue((int)value);
        }
    }
}
#endif
