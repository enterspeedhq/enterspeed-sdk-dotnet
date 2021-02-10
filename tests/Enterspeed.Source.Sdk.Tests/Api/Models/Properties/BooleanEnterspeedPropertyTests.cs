using Enterspeed.Source.Sdk.Api.Models.Properties;
using Xunit;

namespace Enterspeed.Source.Sdk.Tests.Api.Models.Properties
{
    public class BooleanEnterspeedPropertyTests
    {
        [Fact]
        public void TypeIs_Boolean()
        {
            var property = new BooleanEnterspeedProperty("test", false);

            Assert.Equal("boolean", property.Type);
        }

        [Fact]
        public void NameIs_Equal()
        {
            var property = new BooleanEnterspeedProperty("test", false);

            Assert.Equal("test", property.Name);
        }

        [Fact]
        public void NameIs_Null()
        {
            var property = new BooleanEnterspeedProperty(true);

            Assert.Null(property.Name);
        }

        [Fact]
        public void ValueIs_Equal()
        {
            var property = new BooleanEnterspeedProperty("test", true);

            Assert.True(property.Value);
        }
    }
}