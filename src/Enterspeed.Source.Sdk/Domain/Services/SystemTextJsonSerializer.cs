#if NETSTANDARD2_0
using System.Text.Json;
#endif

using System;
using Enterspeed.Source.Sdk.Api.Services;

namespace Enterspeed.Source.Sdk.Domain.Services
{
    public class SystemTextJsonSerializer : IJsonSerializer
    {
#if NETSTANDARD2_0
            private readonly JsonSerializerOptions _options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            public string Serialize(object value)
            {
                return JsonSerializer.Serialize(value, _options);
            }

            public T Deserialize<T>(string value)
            {
                return JsonSerializer.Deserialize<T>(value, _options);
            }
#else
        public string Serialize(object value)
        {
            throw new NotSupportedException("System.Text.Json is only supperted for .NET Standard 2.0");
        }

        public T Deserialize<T>(string value)
        {
            throw new NotSupportedException("System.Text.Json is only supperted for .NET Standard 2.0");
        }
#endif
    }
}