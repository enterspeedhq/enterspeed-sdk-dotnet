using System.Collections.Generic;
using Enterspeed.Source.Sdk.Api.Models.Properties;
using Xunit;

namespace Enterspeed.Source.Sdk.Tests.Api.Models.Properties
{
    public class ObjectEnterspeedPropertyTests
    {
        [Fact]
        public void TypeIs_Array()
        {
            var property = new ObjectEnterspeedProperty("test", new Dictionary<string, IEnterspeedProperty>());

            Assert.Equal("object", property.Type);
        }

        [Fact]
        public void NameIs_Equal()
        {
            var property = new ObjectEnterspeedProperty("test", new Dictionary<string, IEnterspeedProperty>());

            Assert.Equal("test", property.Name);
        }

        [Fact]
        public void NameIs_Null()
        {
            var property = new ObjectEnterspeedProperty(new Dictionary<string, IEnterspeedProperty>());

            Assert.Null(property.Name);
        }

        [Fact]
        public void PropertiesAre_NotEmpty()
        {
            var property = new ObjectEnterspeedProperty(
                "test",
                new Dictionary<string, IEnterspeedProperty>()
                {
                    ["test"] = new StringEnterspeedProperty("test", "value")
                });

            Assert.NotEmpty(property.Properties);
        }
    }
}