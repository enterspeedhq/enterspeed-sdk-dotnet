using System.Net;

namespace Enterspeed.Source.Sdk.Domain.Connection
{
    public class IngestResponse
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
    }
}
