using System;
using System.Net.Http;
using Enterspeed.Source.Sdk.Api.Services;

namespace Enterspeed.Source.Sdk.Domain.Connection
{
    public class EnterspeedConnection
    {
        private readonly IEnterspeedConfigurationService _configurationService;
        private HttpClient _httpClientConnection;
        private DateTime? _connectionEstablishedDate;

        public EnterspeedConnection(IEnterspeedConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public string ApiKey => _configurationService.GetConfiguration().ApiKey;
        public string BaseUrl => _configurationService.GetConfiguration().BaseUrl;
        public int ConnectionTimeout => _configurationService.GetConfiguration().ConnectionTimeout;

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
            _httpClientConnection.BaseAddress = new Uri(new Uri($"{BaseUrl}"), "/api/");
            _httpClientConnection.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);
            _httpClientConnection.DefaultRequestHeaders.Add("Accept", "application/json");

            _connectionEstablishedDate = DateTime.Now;
        }
    }
}
