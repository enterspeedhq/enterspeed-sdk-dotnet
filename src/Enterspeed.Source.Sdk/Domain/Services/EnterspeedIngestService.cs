using System;
using System.Collections.Generic;
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
        private readonly string _ingestEndpointV2;

        public EnterspeedIngestService(
            IEnterspeedConnection connection,
            IJsonSerializer jsonSerializer,
            IEnterspeedConfigurationProvider configurationProvider)
        {
            _connection = connection;
            _jsonSerializer = jsonSerializer;
            _ingestEndpoint = $"/ingest/v{configurationProvider.Configuration.IngestVersion}";
            _ingestEndpointV2 = "/ingest/v2";
        }

        public Response Save(IEnterspeedSourceEntity entity)
        {
            return Save(entity, _connection);
        }

        public Response Save(IEnterspeedSourceEntity entity, IEnterspeedConnection connection)
        {
            var ingestEntity = new EnterspeedSourceEntity<object>(entity.Id, entity.Type, entity.Properties)
            {
                Url = entity.Url,
                Redirects = entity.Redirects,
                ParentId = entity.ParentId
            };

            return Save(ingestEntity, connection);
        }

        public Response Save<T>(IEnterspeedSourceEntity<T> entity)
        {
            return Save(entity, _connection);
        }

        public Response Save<T>(IEnterspeedSourceEntity<T> entity, IEnterspeedConnection connection)
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

            if (string.IsNullOrWhiteSpace(entity.Id))
            {
                return new Response
                {
                    Success = false,
                    Exception = new ArgumentException($"{nameof(entity)}.{nameof(entity.Id)} is required"),
                    Message = $"The required property '{nameof(entity.Id)}' on {nameof(entity)} is missing a value"
                };
            }

            if (string.IsNullOrWhiteSpace(entity.Type))
            {
                return new Response
                {
                    Success = false,
                    Exception = new ArgumentException($"{nameof(entity)}.{nameof(entity.Type)} is required"),
                    Message = $"The required property '{nameof(entity.Type)}' is missing a value"
                };
            }

            var ingestEntity = (EnterspeedSourceEntity<object>)entity;
            // If properties is of type string, we expect a json string that we do not want to serialize once again
            // so we create a new entity with the deserialized properties as a Dictionary<string, object>
            if (entity.Properties is string entityProperties)
            {
                var properties = _jsonSerializer.Deserialize<IDictionary<string, object>>(entityProperties);
                ingestEntity = new EnterspeedSourceEntity<object>(entity.Id, entity.Type, properties)
                {
                    Url = entity.Url,
                    Redirects = entity.Redirects,
                    ParentId = entity.ParentId
                };
            }
            
            var serializedEntity = _jsonSerializer.Serialize(ingestEntity);

            return Ingest(serializedEntity, $"{_ingestEndpointV2}/{ingestEntity.Id}", connection);
        }

        public Response Save(IEnterspeedEntity entity)
        {
            return Save(entity, _connection);
        }

        public Response Save(IEnterspeedEntity entity, IEnterspeedConnection connection)
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

            var content = _jsonSerializer.Serialize(entity);
            return Ingest(content, _ingestEndpoint, connection);
        }

        private Response Ingest(string jsonEntityToIngest, string ingestUrl, IEnterspeedConnection connection)
        {
            HttpResponseMessage response = null;
            IngestResponse ingestResponse = null;
            string responseContentAsString = null;
            try
            {
                var buffer = Encoding.UTF8.GetBytes(jsonEntityToIngest);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                response = connection.HttpClientConnection.PostAsync(ingestUrl, byteContent)
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
            return Delete(id, _connection);
        }

        public Response Delete(string id, IEnterspeedConnection connection)
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
                response = connection.HttpClientConnection
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
