using System;
using System.Net.Http;
using Enterspeed.Source.Sdk.Api.Connection;
using Enterspeed.Source.Sdk.Api.Providers;

namespace Enterspeed.Source.Sdk.Domain.Connection
{
    public class EnterspeedConnection : IEnterspeedConnection
    {
        private readonly IEnterspeedConfigurationProvider _configurationProvider;
        private HttpClient _httpClientConnection;
        private DateTime? _connectionEstablishedDate;

        public EnterspeedConnection(IEnterspeedConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        private string ApiKey => _configurationProvider.Configuration.ApiKey;
        private string BaseUrl => _configurationProvider.Configuration.BaseUrl;
        private int ConnectionTimeout => _configurationProvider.Configuration.ConnectionTimeout;
        private string IngestVersion => _configurationProvider.Configuration.IngestVersion;

        public HttpClient HttpClientConnection
        {
            get
            {
                if (_httpClientConnection == null
                    || !_connectionEstablishedDate.HasValue
                    || ((DateTime.Now - _connectionEstablishedDate.Value).TotalSeconds > ConnectionTimeout))
                {
                    Connect();
                }

                return _httpClientConnection;
            }
        }

        public void Flush()
        {
            _httpClientConnection = null;
            _connectionEstablishedDate = null;
        }

        private void Connect()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                throw new Exception("ApiKey is missing in the connection.");
            }

            if (string.IsNullOrWhiteSpace(BaseUrl))
            {
                throw new Exception("BaseUrl is missing in the connection.");
            }

            _httpClientConnection = new HttpClient();
            _httpClientConnection.BaseAddress = new Uri(BaseUrl);
            _httpClientConnection.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);
            _httpClientConnection.DefaultRequestHeaders.Add("Accept", "application/json");

            _connectionEstablishedDate = DateTime.Now;
        }
    }
}
