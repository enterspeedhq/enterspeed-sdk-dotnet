#if NETSTANDARD2_0 || NET6_0_OR_GREATER
using Enterspeed.Source.Sdk.Api.Connection;
using Enterspeed.Source.Sdk.Api.Providers;
using Enterspeed.Source.Sdk.Api.Services;
using Enterspeed.Source.Sdk.Configuration;
using Enterspeed.Source.Sdk.Domain.Connection;
using Enterspeed.Source.Sdk.Domain.Providers;
using Enterspeed.Source.Sdk.Domain.Services;
using Enterspeed.Source.Sdk.Domain.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;

namespace Enterspeed.Source.Sdk.Extensions.Setup
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEnterspeedIngestService(
            this IServiceCollection services,
            EnterspeedConfiguration configuration)
        {
            services.AddScoped<IEnterspeedConnection, EnterspeedConnection>();
            services.AddScoped<IEnterspeedIngestService, EnterspeedIngestService>();
            services.AddScoped<IJsonSerializer, SystemTextJsonSerializer>();
            services.AddSingleton<IEnterspeedConfigurationProvider>(
                new EnterspeedConfigurationProvider(configuration));

            return services;
        }
    }
}
#endif
