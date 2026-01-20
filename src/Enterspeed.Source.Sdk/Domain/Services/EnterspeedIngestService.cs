using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Enterspeed.Source.Sdk.Api.Connection;
using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Models.Bulk;
using Enterspeed.Source.Sdk.Api.Models.Properties;
using Enterspeed.Source.Sdk.Api.Services;
using Enterspeed.Source.Sdk.Domain.Connection;

namespace Enterspeed.Source.Sdk.Domain.Services
{
    public class EnterspeedIngestService : IEnterspeedIngestService
    {
        private readonly IEnterspeedConnection _connection;
        private readonly IJsonSerializer _jsonSerializer;

        public EnterspeedIngestService(
            IEnterspeedConnection connection,
            IJsonSerializer jsonSerializer)
        {
            _connection = connection;
            _jsonSerializer = jsonSerializer;
        }

        public Response Save(IEnterspeedEntity entity)
        {
            return Save(entity, _connection);
        }

        public Response Save(IEnterspeedEntity entity, IEnterspeedConnection connection)
        {
            var ingestEntity = new EnterspeedEntity<object>(entity.Id, entity.Type, entity.Properties)
            {
                Url = entity.Url,
                Redirects = entity.Redirects,
                ParentId = entity.ParentId
            };

            return Save(ingestEntity, connection);
        }

        public Response Save<T>(IEnterspeedEntity<T> entity)
        {
            return Save(entity, _connection);
        }

        public Response Save<T>(IEnterspeedEntity<T> entity, IEnterspeedConnection connection)
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

            // This was the default approach. We expect that everything is taken care of here.
            if (entity.Properties is IDictionary<string, IEnterspeedProperty>)
            {
                return Ingest(entity, IngestVersion.V1, connection);
            }

            // If properties is of type string, we expect a json string that we do not want to serialize once again
            // so we create a new entity with the deserialized properties as a Dictionary<string, object>
            if (entity.Properties is string entityProperties)
            {
                var properties = _jsonSerializer.Deserialize<IDictionary<string, object>>(entityProperties);
                var ingestEntity = new EnterspeedEntity<object>(entity.Id, entity.Type, properties)
                {
                    Url = entity.Url,
                    Redirects = entity.Redirects,
                    ParentId = entity.ParentId
                };

                return Ingest(ingestEntity, IngestVersion.V2, connection);
            }

            return Ingest(entity, IngestVersion.V2, connection);
        }

        private Response Ingest<T>(IEnterspeedEntity<T> entity, IngestVersion ingestVersion, IEnterspeedConnection connection)
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
                string jsonEntityToIngest;
                string ingestUrl;
                if (ingestVersion == IngestVersion.V1)
                {
                    jsonEntityToIngest = _jsonSerializer.Serialize(entity);
                    ingestUrl = GetIngestUrl(IngestVersion.V1);
                }
                else
                {
                    var enterspeedEntityV2 = new EnterspeedEntityV2<T>(entity);
                    jsonEntityToIngest = _jsonSerializer.Serialize(enterspeedEntityV2);
                    ingestUrl = GetIngestUrl(IngestVersion.V2, entity.Id);
                }

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
                    .DeleteAsync(GetIngestUrl(IngestVersion.V2, id))
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
                    .PostAsync(GetIngestUrl(IngestVersion.V2, "123"), byteContent)
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

        public BulkIngestResponse SaveBulk(IEnumerable<BulkIngestEntity> entities)
        {
            return SaveBulk(entities, _connection);
        }

        public BulkIngestResponse SaveBulk(IEnumerable<BulkIngestEntity> entities, IEnterspeedConnection connection)
        {
            if (entities == null)
            {
                return new BulkIngestResponse
                {
                    Success = false,
                    Exception = new ArgumentNullException(nameof(entities)),
                    Status = HttpStatusCode.BadRequest
                };
            }

            var entitiesList = entities.ToList();

            if (!entitiesList.Any())
            {
                return new BulkIngestResponse
                {
                    Success = true,
                    Status = HttpStatusCode.OK
                };
            }

            HttpResponseMessage response = null;
            string responseContentAsString = null;

            try
            {
                var jsonEntityArrayToIngest = _jsonSerializer.Serialize(entitiesList);
                var buffer = Encoding.UTF8.GetBytes(jsonEntityArrayToIngest);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                response = connection.HttpClientConnection
                    .PostAsync("/ingest/v2", byteContent)
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
                    var bulkResponse = _jsonSerializer.Deserialize<BulkIngestResponse>(responseContentAsString);
                    if (bulkResponse != null)
                    {
                        bulkResponse.Status = response.StatusCode;
                        bulkResponse.Success = response.IsSuccessStatusCode;
                        bulkResponse.Content = responseContentAsString;
                        return bulkResponse;
                    }
                }
            }
            catch (Exception e)
            {
                return new BulkIngestResponse
                {
                    Success = false,
                    Status = response?.StatusCode ?? HttpStatusCode.BadRequest,
                    Exception = e,
                    Content = responseContentAsString
                };
            }

            if (response == null)
            {
                return new BulkIngestResponse
                {
                    Success = false,
                    Status = HttpStatusCode.BadRequest,
                    Exception = new Exception("Failed sending bulk ingest data")
                };
            }

            return new BulkIngestResponse
            {
                Status = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Content = responseContentAsString
            };
        }

        public BulkDeleteResponse DeleteBulk(IEnumerable<string> originIds)
        {
            return DeleteBulk(originIds, _connection);
        }

        public BulkDeleteResponse DeleteBulk(IEnumerable<string> originIds, IEnterspeedConnection connection)
        {
            if (originIds == null)
            {
                return new BulkDeleteResponse
                {
                    Success = false,
                    Exception = new ArgumentNullException(nameof(originIds)),
                    Status = HttpStatusCode.BadRequest
                };
            }

            var request = new BulkDeleteRequest(originIds);
            return DeleteBulk(request, connection);
        }

        public BulkDeleteResponse DeleteBulk(BulkDeleteRequest request)
        {
            return DeleteBulk(request, _connection);
        }

        public BulkDeleteResponse DeleteBulk(BulkDeleteRequest request, IEnterspeedConnection connection)
        {
            if (request == null)
            {
                return new BulkDeleteResponse
                {
                    Success = false,
                    Exception = new ArgumentNullException(nameof(request)),
                    Status = HttpStatusCode.BadRequest
                };
            }

            if (request.OriginIds == null || !request.OriginIds.Any())
            {
                return new BulkDeleteResponse
                {
                    Success = true,
                    Status = HttpStatusCode.OK
                };
            }

            HttpResponseMessage response = null;
            string responseContentAsString = null;

            try
            {
                var jsonRequestBody = _jsonSerializer.Serialize(request);
                var httpRequest = new HttpRequestMessage(HttpMethod.Delete, "/ingest/v2")
                {
                    Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json")
                };

                response = connection.HttpClientConnection
                    .SendAsync(httpRequest)
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
                    var bulkDeleteResponse = _jsonSerializer.Deserialize<BulkDeleteResponse>(responseContentAsString);
                    if (bulkDeleteResponse != null)
                    {
                        bulkDeleteResponse.Status = response.StatusCode;
                        bulkDeleteResponse.Success = response.IsSuccessStatusCode;
                        bulkDeleteResponse.Content = responseContentAsString;
                        return bulkDeleteResponse;
                    }
                }
            }
            catch (Exception e)
            {
                return new BulkDeleteResponse
                {
                    Success = false,
                    Status = response?.StatusCode ?? HttpStatusCode.BadRequest,
                    Exception = e,
                    Content = responseContentAsString
                };
            }

            if (response == null)
            {
                return new BulkDeleteResponse
                {
                    Success = false,
                    Status = HttpStatusCode.BadRequest,
                    Exception = new Exception("Failed sending bulk delete data")
                };
            }

            return new BulkDeleteResponse
            {
                Status = response.StatusCode,
                Success = response.IsSuccessStatusCode,
                Content = responseContentAsString
            };
        }

        private static string GetIngestUrl(IngestVersion version, string sourceEntityId = null)
        {
            var ingestUrl = $"/ingest/v{(int)version}";

            if (!string.IsNullOrWhiteSpace(sourceEntityId))
            {
                ingestUrl += version == IngestVersion.V1
                    ? $"?id={sourceEntityId}"
                    : $"/{sourceEntityId}";
            }

            return ingestUrl;
        }
    }
}