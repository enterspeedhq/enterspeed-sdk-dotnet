#if NETSTANDARD2_0 || NET6_0
using System.Text.Json;
using Enterspeed.Source.Sdk.Api.Services;
namespace Enterspeed.Source.Sdk.Domain.SystemTextJson
{
    public class SystemTextJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public SystemTextJsonSerializer()
        {
            _options.Converters.Add(new EnterspeedPropertyConverter());
            _options.Converters.Add(new HttpStatusCodeConverter());
        }

        public string Serialize(object value)
        {
            return JsonSerializer.Serialize(value, _options);
        }

        public T Deserialize<T>(string value)
        {
            return JsonSerializer.Deserialize<T>(value, _options);
        }
    }
}
#endif
