using Enterspeed.Source.Sdk.Configuration;

namespace Enterspeed.Source.Sdk.Api.Providers
{
    public interface IEnterspeedConfigurationProvider
    {
        EnterspeedConfiguration Configuration { get; }
    }
}