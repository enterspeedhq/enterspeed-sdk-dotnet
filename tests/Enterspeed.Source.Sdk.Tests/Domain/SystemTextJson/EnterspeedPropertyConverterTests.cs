using System.Collections.Generic;
using System.Text.Json;
using Enterspeed.Source.Sdk.Api.Models.Properties;
using Enterspeed.Source.Sdk.Domain.SystemTextJson;
using Enterspeed.Source.Sdk.Tests.Mock;
using Xunit;

namespace Enterspeed.Source.Sdk.Tests.Domain.SystemTextJson
{
    public class EnterspeedPropertyConverterTests
    {
        [Fact]
        public void CanWrite_String_Contains()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnterspeedPropertyConverter());

            var value = new MockEnterspeedEntity
            {
                Properties = new Dictionary<string, IEnterspeedProperty>()
                {
                    ["title"] = new StringEnterspeedProperty("title", "Hello world")
                }
            };

            var result = JsonSerializer.Serialize(value, options);

            Assert.Contains("Hello world", result);
        }

        [Fact]
        public void CanWrite_Integer_Contains()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnterspeedPropertyConverter());

            var value = new MockEnterspeedEntity
            {
                Properties = new Dictionary<string, IEnterspeedProperty>()
                {
                    ["price"] = new NumberEnterspeedProperty("price", 11.95)
                }
            };

            var result = JsonSerializer.Serialize(value, options);

            Assert.Contains("11.95", result);
        }

        [Fact]
        public void CanWrite_Boolean_Contains()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnterspeedPropertyConverter());

            var value = new MockEnterspeedEntity
            {
                Id = "id",
                Type = "type",
                Properties = new Dictionary<string, IEnterspeedProperty>()
                {
                    ["isValid"] = new BooleanEnterspeedProperty("isValid", true)
                }
            };

            var result = JsonSerializer.Serialize(value, options);

            Assert.Contains("true", result);
        }

        [Fact]
        public void CanWrite_Array_Contains()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnterspeedPropertyConverter());

            var items = new[]
            {
                new StringEnterspeedProperty("title", "Hello world")
            };

            var value = new MockEnterspeedEntity
            {
                Properties = new Dictionary<string, IEnterspeedProperty>()
                {
                    ["arrayItems"] = new ArrayEnterspeedProperty("arrayItems", items)
                }
            };

            var result = JsonSerializer.Serialize(value, options);

            Assert.Contains("Hello world", result);
        }

        [Fact]
        public void CanWrite_Object_Contains()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnterspeedPropertyConverter());

            var value = new MockEnterspeedEntity
            {
                Properties = new Dictionary<string, IEnterspeedProperty>()
                {
                    ["items"] = new ObjectEnterspeedProperty("items", new Dictionary<string, IEnterspeedProperty>
                    {
                        ["title"] = new StringEnterspeedProperty("title", "Hello world")
                    })
                }
            };

            var result = JsonSerializer.Serialize(value, options);

            Assert.Contains("Hello world", result);
        }
    }
}