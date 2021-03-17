using System.Net;
using System.Text.Json;
using Enterspeed.Source.Sdk.Domain.Connection;
using Enterspeed.Source.Sdk.Domain.SystemTextJson;
using Xunit;

namespace Enterspeed.Source.Sdk.Tests.Domain.SystemTextJson
{
    public class HttpStatusCodeConvertTests
    {
        [Fact]
        public void CanWrite_Contains()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            options.Converters.Add(new HttpStatusCodeConverter());

            var value = new
            {
                status = HttpStatusCode.BadRequest
            };

            var result = JsonSerializer.Serialize(value, options);

            Assert.Contains("400", result);
        }

        [Fact]
        public void CanRead_Equal()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            options.Converters.Add(new HttpStatusCodeConverter());

            var result = JsonSerializer.Deserialize<IngestResponse>("{\"status\": 200, \"message\": \"Entity Saved\"}", options);

            Assert.Equal(HttpStatusCode.OK, result.Status);
        }
    }
}