using Enterspeed.Source.Sdk.Api.Services;
using Enterspeed.Source.Sdk.Configuration;

namespace Enterspeed.Source.Sdk.Domain.Services
{
    public class InMemoryEnterspeedConfigurationService : IEnterspeedConfigurationService
    {
        private EnterspeedConfiguration _configuration;

        public InMemoryEnterspeedConfigurationService(EnterspeedConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Save(EnterspeedConfiguration configuration)
        {
            _configuration = configuration;
        }

        public EnterspeedConfiguration GetConfiguration()
        {
            return _configuration;
        }
    }
}
