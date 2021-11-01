using System.Collections.Generic;
using System.Net;

namespace Enterspeed.Source.Sdk.Domain.Connection
{
    public class IngestResponse
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public string ErrorCode { get; set; }
    }
}
