using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Domain.Client;

namespace Enterspeed.Source.Sdk.Api.Services
{
    public interface IEnterspeedIngestService
    {
        Response Save(IEnterspeedEntity entity);
        Response Delete(string id);
    }
}
