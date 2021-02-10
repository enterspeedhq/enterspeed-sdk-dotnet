using Enterspeed.Source.Sdk.Api.Models.Properties;
using Xunit;

namespace Enterspeed.Source.Sdk.Tests.Api.Models.Properties
{
    public class StringEnterspeedPropertyTests
    {
        [Fact]
        public void TypeIs_Number()
        {
            var property = new StringEnterspeedProperty("test", "value");

            Assert.Equal("string", property.Type);
        }

        [Fact]
        public void NameIs_Equal()
        {
            var property = new StringEnterspeedProperty("test", "value");

            Assert.Equal("test", property.Name);
        }

        [Fact]
        public void NameIs_Null()
        {
            var property = new StringEnterspeedProperty("value");

            Assert.Null(property.Name);
        }

        [Fact]
        public void ValueIs_Equal()
        {
            var property = new StringEnterspeedProperty("test", "value");

            Assert.Equal("value", property.Value);
        }
    }
}