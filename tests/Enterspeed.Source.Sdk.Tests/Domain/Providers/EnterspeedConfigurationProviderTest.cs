using System;
using Enterspeed.Source.Sdk.Configuration;
using Enterspeed.Source.Sdk.Domain.Providers;
using Xunit;

namespace Enterspeed.Source.Sdk.Tests.Domain.Providers
{
    public class EnterspeedConfigurationProviderTest
    {
        [Fact]
        public void ConfigurationProvider_Success()
        {
            var configuration = new EnterspeedConfiguration();
            var configurationProvider = new EnterspeedConfigurationProvider(configuration);

            Assert.Equal(configuration, configurationProvider.Configuration);
        }

        [Fact]
        public void ConfigurationProvider_MissingConfiguration()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new EnterspeedConfigurationProvider(null));

            Assert.Equal("configuration", exception.ParamName);
        }
    }
}
