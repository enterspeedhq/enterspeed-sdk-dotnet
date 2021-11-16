#if NETSTANDARD2_0_OR_GREATER
using System;
using Enterspeed.Source.Sdk.Api.Connection;
using Enterspeed.Source.Sdk.Api.Providers;
using Enterspeed.Source.Sdk.Api.Services;
using Enterspeed.Source.Sdk.Configuration;
using Enterspeed.Source.Sdk.Domain.Connection;
using Enterspeed.Source.Sdk.Domain.Providers;
using Enterspeed.Source.Sdk.Domain.Services;
using Enterspeed.Source.Sdk.Domain.SystemTextJson;
using Enterspeed.Source.Sdk.Extensions.NETCore.Setup;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Enterspeed.Source.Sdk.Tests.Extensions
{
    public class NETCoreSetupTest
    {
        [Fact]
        public void AddEnterspeedIngestService_Success()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            var configuration = new EnterspeedConfiguration
            {
                ApiKey = "source-" + Guid.NewGuid(),
                BaseUrl = "https://example.com"
            };

            serviceCollection.AddEnterspeedIngestService(configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var ingestService = serviceProvider.GetService<IEnterspeedIngestService>();
            var connection = serviceProvider.GetService<IEnterspeedConnection>();
            var serializer = serviceProvider.GetService<IJsonSerializer>();
            var configurationProvider = serviceProvider.GetService<IEnterspeedConfigurationProvider>();

            Assert.IsType<EnterspeedIngestService>(ingestService);
            Assert.IsType<EnterspeedConnection>(connection);
            Assert.IsType<SystemTextJsonSerializer>(serializer);
            Assert.IsType<EnterspeedConfigurationProvider>(configurationProvider);

            Assert.Equal(configuration, configurationProvider.Configuration);
        }

        [Fact]
        public void AddEnterspeedIngestService_MissingConfiguration()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            Assert.Throws<ArgumentNullException>(() => serviceCollection.AddEnterspeedIngestService(null));
        }
    }
}
#endif
