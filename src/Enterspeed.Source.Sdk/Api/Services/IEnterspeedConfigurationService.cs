using Enterspeed.Source.Sdk.Configuration;

namespace Enterspeed.Source.Sdk.Api.Services
{
    public interface IEnterspeedConfigurationService
    {
        void Save(EnterspeedConfiguration configuration);
        EnterspeedConfiguration GetConfiguration();
    }
}
