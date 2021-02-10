using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Enterspeed.Source.Sdk.Tests.Mock
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _response;
        private readonly HttpStatusCode _statusCode;
        private readonly bool _throwException;

        public string Input { get; private set; }
        public int NumberOfCalls { get; private set; }

        public MockHttpMessageHandler(string response, HttpStatusCode statusCode, bool throwException = false)
        {
            _response = response;
            _statusCode = statusCode;
        }

        public MockHttpMessageHandler(HttpStatusCode statusCode, bool throwException = false)
        {
            _statusCode = statusCode;
            _throwException = throwException;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_throwException)
            {
                throw new Exception("FAILED");
            }

            NumberOfCalls++;
            if (request.Content != null)
            {
                Input = await request.Content.ReadAsStringAsync();
            }

            return new HttpResponseMessage
            {
                StatusCode = _statusCode,
                Content = new StringContent(_response ?? string.Empty)
            };
        }
    }
}