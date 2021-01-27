using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Services;
using Enterspeed.Source.Sdk.Domain.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Enterspeed.Source.Sdk.Domain.Services
{
    public class EnterspeedIngestService : IEnterspeedIngestService
    {
        private readonly EnterspeedConnection _connection;

        public EnterspeedIngestService(EnterspeedConnection connection)
        {
            _connection = connection;
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

            try
            {
                var content = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                var buffer = Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                response = _connection.HttpClientConnection.PostAsync(
                        "ingest",
                        byteContent)
                    .Result;

                var ingestResponseJson = response?.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrWhiteSpace(ingestResponseJson))
                {
                    ingestResponse = JsonConvert.DeserializeObject<IngestResponse>(ingestResponseJson);
                }
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
                    Exception = new Exception("Failed sending data")
                };
            }

            var statusCode = ingestResponse?.Status ?? response.StatusCode;

            return new Response
            {
                Message = ingestResponse?.Message,
                Status = statusCode,
                Success = statusCode == HttpStatusCode.OK
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
                response = _connection.HttpClientConnection.DeleteAsync(
                        $"ingest?id={id}")
                    .Result;
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
                Message = response.Content.ReadAsStringAsync().Result,
                Status = response.StatusCode,
                Success = response.StatusCode == HttpStatusCode.OK
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
                response = _connection.HttpClientConnection.PostAsync(
                    "ingest",
                    byteContent).Result;
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
                Success = statusCode == HttpStatusCode.OK
            };
        }
    }
}
