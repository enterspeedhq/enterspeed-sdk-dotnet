using System;
using System.Net;

namespace Enterspeed.Source.Sdk.Domain.Connection
{
    public class Response
    {
        public HttpStatusCode Status { get; set; }
        public int StatusCode => Status.GetHashCode();
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public bool Success { get; set; }
    }
}
