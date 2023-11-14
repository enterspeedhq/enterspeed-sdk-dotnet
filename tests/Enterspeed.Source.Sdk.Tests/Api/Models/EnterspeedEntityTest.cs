using Enterspeed.Source.Sdk.Api.Models;
using Xunit;

namespace Enterspeed.Source.Sdk.Tests.Api.Models
{
    public class EnterspeedEntityTest
    {
        [Fact]
        public void EnterspeedEntity_ImplementsInterface()
        {
            var entity = new EnterspeedEntity("id", "type");

            Assert.IsAssignableFrom<IEnterspeedEntity>(entity);
        }
    }
}
