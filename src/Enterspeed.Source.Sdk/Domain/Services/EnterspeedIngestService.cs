using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Enterspeed.Source.Sdk.Api.Connection;
using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Providers;
using Enterspeed.Source.Sdk.Api.Services;
using Enterspeed.Source.Sdk.Domain.Connection;

namespace Enterspeed.Source.Sdk.Domain.Services
{
    public class EnterspeedIngestService : IEnterspeedIngestService
    {
        private readonly IEnterspeedConnection _connection;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly string _ingestEndpoint;

        public EnterspeedIngestService(
            IEnterspeedConnection connection,
            IJsonSerializer jsonSerializer,
            IEnterspeedConfigurationProvider configurationProvider)
        {
            _connection = connection;
            _jsonSerializer = jsonSerializer;
            _ingestEndpoint = $"/ingest/v{configurationProvider.Configuration.IngestVersion}";
        }

        public Response Save(IEnterspeedEntity entity)
        {
            if (entity == null)
            {
                return new Response
                {
                    Success = false,
                    Exception = new ArgumentNullException(nameof(entity)),
                    Message = "Missing entity"
                };
            }

            HttpResponseMessage response = null;
            IngestResponse ingestResponse = null;
            string responseContentAsString = null;
            try
            {
                var content = _jsonSerializer.Serialize(entity);

                var buffer = Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                response = _connection.HttpClientConnection.PostAsync(_ingestEndpoint, byteContent)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                responseContentAsString = response?.Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
                if (!string.IsNullOrWhiteSpace(responseContentAsString))
                {
                    ingestResponse = _jsonSerializer.Deserialize<IngestResponse>(responseContentAsString);
                }
            }
            catch (Exception e)
            {
                return new Response
                {
                    Success = false,
                    Status = response?.StatusCode ?? HttpStatusCode.BadRequest,
                    Exception = e,
                    Content = responseContentAsString
                };
            }

            if (response == null)
            {
                return new Response
                {
                    Success = false,
                    Exception = new Exception("Failed sending data")
                };
            }

            var statusCode = ingestResponse?.Status ?? response.StatusCode;

            return new Response
            {
                Message = ingestResponse?.Message,
                Errors = ingestResponse?.Errors,
                ErrorCode = ingestResponse?.ErrorCode,
                Status = statusCode,
                Success = response.IsSuccessStatusCode,
                Content = responseContentAsString
            };
        }

        public Response Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new Response
                {
                    Success = false,
                    Exception = new ArgumentNullException(nameof(id)),
                    Message = "Missing entity"
                };
            }

            HttpResponseMessage response = null;
            try
            {
                response = _connection.HttpClientConnection
                    .DeleteAsync($"{_ingestEndpoint}?id={id}")
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception e)
            {
                return new Response
                {
                    Success = false,
                    Status = response?.StatusCode ?? HttpStatusCode.BadRequest,
                    Exception = e
                };
            }

            if (response == null)
            {
                return new Response
                {
                    Success = false,
                    Exception = new Exception("Failed deleting data")
                };
            }

            return new Response
            {
                Message = response.Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult(),
                Status = response.StatusCode,
                Success = response.IsSuccessStatusCode
            };
        }

        public Response Test()
        {
            HttpResponseMessage response = null;
            try
            {
                var buffer = Encoding.UTF8.GetBytes("{}");
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = _connection.HttpClientConnection
                    .PostAsync(_ingestEndpoint, byteContent)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception e)
            {
                return new Response
                {
                    Success = false,
                    Status = response?.StatusCode ?? HttpStatusCode.BadRequest,
                    Exception = e
                };
            }

            if (response == null)
            {
                return new Response
                {
                    Success = false,
                    Exception = new Exception("Failed testing endpoint")
                };
            }

            var statusCode = response.StatusCode;
            return new Response
            {
                Status = statusCode,
                Success = response.IsSuccessStatusCode
            };
        }
    }
}
