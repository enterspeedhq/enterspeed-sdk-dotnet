using System;
using System.Net.Http;
using System.Reflection;
using Enterspeed.Source.Sdk.Api.Connection;
using Enterspeed.Source.Sdk.Api.Providers;
using Enterspeed.Source.Sdk.Configuration;
namespace Enterspeed.Source.Sdk.Domain.Connection
{
    public sealed class EnterspeedConnection : IEnterspeedConnection
    {
        private readonly IEnterspeedConfigurationProvider _configurationProvider;
        private DateTime? _connectionEstablishedDate;
        private HttpClient _httpClientConnection;

        public EnterspeedConnection(IEnterspeedConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        private string ApiKey => _configurationProvider.Configuration.ApiKey;
        private string BaseUrl => _configurationProvider.Configuration.BaseUrl;
        private int ConnectionTimeout => _configurationProvider.Configuration.ConnectionTimeout;

        public HttpClient HttpClientConnection
        {
            get
            {
                if (_httpClientConnection == null
                    || !_connectionEstablishedDate.HasValue
                    || (DateTime.Now - _connectionEstablishedDate.Value).TotalSeconds > ConnectionTimeout)
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

        public void Dispose()
        {
            _httpClientConnection.Dispose();
        }

        private void Connect()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                throw new ConfigurationException(nameof(ApiKey));
            }

            if (string.IsNullOrWhiteSpace(BaseUrl))
            {
                throw new ConfigurationException(nameof(BaseUrl));
            }

            _httpClientConnection = new HttpClient();
            _httpClientConnection.BaseAddress = new Uri(BaseUrl);
            _httpClientConnection.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);
            _httpClientConnection.DefaultRequestHeaders.Add("Accept", "application/json");

#if NETSTANDARD2_0_OR_GREATER || NET || NETCOREAPP2_0_OR_GREATER
            _httpClientConnection.DefaultRequestHeaders.Add("X-Enterspeed-System", $"sdk-dotnet/{Assembly.GetExecutingAssembly().GetName().Version}");
#endif

            _connectionEstablishedDate = DateTime.Now;
        }
    }
}
