using Enterspeed.Source.Sdk.Api.Models.Properties;
using Xunit;

namespace Enterspeed.Source.Sdk.Tests.Api.Models.Properties
{
    public class NumberEnterspeedPropertyTests
    {
        [Fact]
        public void TypeIs_Number()
        {
            var property = new NumberEnterspeedProperty("test", 10);

            Assert.Equal("number", property.Type);
        }

        [Fact]
        public void NameIs_Equal()
        {
            var property = new NumberEnterspeedProperty("test", 10);

            Assert.Equal("test", property.Name);
        }

        [Fact]
        public void NameIs_Null()
        {
            var property = new NumberEnterspeedProperty(10);

            Assert.Null(property.Name);
        }

        [Fact]
        public void ValueIs_Equal()
        {
            var property = new NumberEnterspeedProperty("test", 10);

            Assert.Equal(10, property.Value);
        }

        [Fact]
        public void Precision_Equal()
        {
            var property = new NumberEnterspeedProperty("test", 10.535);

            Assert.Equal(3, property.Precision);
        }

        [Fact]
        public void Precision_Equal_0()
        {
            var property = new NumberEnterspeedProperty("test", 10);

            Assert.Equal(0, property.Precision);
        }

        [Fact]
        public void Precision_Consistent()
        {
            var property = new NumberEnterspeedProperty("test", 10.2);

            Assert.Equal(1, property.Precision);
            Assert.Equal(1, property.Precision);
        }
    }
}