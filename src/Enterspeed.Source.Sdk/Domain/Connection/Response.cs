using System;
using System.Collections.Generic;
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
        public Dictionary<string, string> Errors { get; set; }
        public string ErrorCode { get; set; }
        public string ResponseContent { get; set; }
    }
}