using System.Net;

namespace Enterspeed.Source.Sdk.Domain.Client
{
    public class IngestResponse
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
    }
}
