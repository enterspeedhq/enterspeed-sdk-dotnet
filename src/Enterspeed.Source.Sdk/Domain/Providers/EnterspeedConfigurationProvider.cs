using System;
using Enterspeed.Source.Sdk.Api.Providers;
using Enterspeed.Source.Sdk.Configuration;

namespace Enterspeed.Source.Sdk.Domain.Providers
{
    public class EnterspeedConfigurationProvider : IEnterspeedConfigurationProvider
    {
        public EnterspeedConfiguration Configuration { get; }

        public EnterspeedConfigurationProvider(EnterspeedConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
    }
}
