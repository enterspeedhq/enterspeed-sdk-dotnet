using Enterspeed.Source.Sdk.Api.Models.Properties;
using Xunit;

namespace Enterspeed.Source.Sdk.Tests.Api.Models.Properties
{
    public class ArrayEnterspeedPropertyTests
    {
        [Fact]
        public void TypeIs_Array()
        {
            var property = new ArrayEnterspeedProperty("test", new IEnterspeedProperty[0]);

            Assert.Equal("array", property.Type);
        }

        [Fact]
        public void NameIs_Equal()
        {
            var property = new ArrayEnterspeedProperty("test", new IEnterspeedProperty[0]);

            Assert.Equal("test", property.Name);
        }

        [Fact]
        public void ItemsAre_NotEmpty()
        {
            var property = new ArrayEnterspeedProperty(
                "test",
                new IEnterspeedProperty[] { new StringEnterspeedProperty("test", "value") });

            Assert.NotEmpty(property.Items);
        }
    }
}