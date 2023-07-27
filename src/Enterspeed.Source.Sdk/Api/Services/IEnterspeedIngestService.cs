using Enterspeed.Source.Sdk.Api.Connection;
using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Domain.Connection;

namespace Enterspeed.Source.Sdk.Api.Services
{
    public interface IEnterspeedIngestService
    {
        Response Save(IEnterspeedJsonEntity entity);
        Response Save(IEnterspeedJsonEntity entity, IEnterspeedConnection connection);
        Response Save(IEnterspeedEntity entity);
        Response Save(IEnterspeedEntity entity, IEnterspeedConnection connection);
        Response Delete(string id);
        Response Delete(string id, IEnterspeedConnection connection);
    }
}
