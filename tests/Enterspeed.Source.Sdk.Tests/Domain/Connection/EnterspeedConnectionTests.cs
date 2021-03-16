using System;
using System.Linq;
using System.Threading;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Enterspeed.Source.Sdk.Api.Providers;
using Enterspeed.Source.Sdk.Configuration;
using Enterspeed.Source.Sdk.Domain.Connection;
using NSubstitute;
using Xunit;

namespace Enterspeed.Source.Sdk.Tests.Domain.Connection
{
    public class EnterspeedConnectionTests
    {
        public class EnterspeedConnectionTestFixture : Fixture
        {
            public IEnterspeedConfigurationProvider ConfigurationProvider { get; set; }
            public EnterspeedConnectionTestFixture()
            {
                Customize(new AutoNSubstituteCustomization());

                ConfigurationProvider = this.Freeze<IEnterspeedConfigurationProvider>();
            }
        }

        public class HttpClientConnection
        {
            [Fact]
            public void EstablishesConnectionIfNull_NotNull()
            {
                var fixture = new EnterspeedConnectionTestFixture();

                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration()
                    {
                        ApiKey = "source-" + Guid.NewGuid(),
                        BaseUrl = "https://example.com/"
                    });

                var sut = fixture.Create<EnterspeedConnection>();

                Assert.NotNull(sut.HttpClientConnection);
            }

            [Fact]
            public void MissingApiKey_Throws()
            {
                var fixture = new EnterspeedConnectionTestFixture();

                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration()
                    {
                        BaseUrl = "https://example.com/"
                    });

                var sut = fixture.Create<EnterspeedConnection>();

                Assert.Throws<Exception>(() => sut.HttpClientConnection);
            }

            [Fact]
            public void MissingBaseUrl_Throws()
            {
                var fixture = new EnterspeedConnectionTestFixture();

                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration()
                    {
                        ApiKey = "source-" + Guid.NewGuid()
                    });

                var sut = fixture.Create<EnterspeedConnection>();

                Assert.Throws<Exception>(() => sut.HttpClientConnection);
            }

            [Fact]
            public void ApiKeyIsAddedToHeaders_Equal()
            {
                var fixture = new EnterspeedConnectionTestFixture();

                var apiKey = "source-" + Guid.NewGuid();
                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration()
                    {
                        ApiKey = apiKey,
                        BaseUrl = "https://example.com/"
                    });

                var sut = fixture.Create<EnterspeedConnection>();

                Assert.Equal(apiKey, sut.HttpClientConnection.DefaultRequestHeaders.GetValues("X-Api-Key").First());
            }

            [Fact]
            public void ConnectionIsResetWhenTimeout_NotEqual()
            {
                var fixture = new EnterspeedConnectionTestFixture();

                var enterspeedConfiguration = new EnterspeedConfiguration()
                {
                    ApiKey = "source-" + Guid.NewGuid(),
                    BaseUrl = "https://example.com/",
                    ConnectionTimeout = 1
                };

                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(enterspeedConfiguration);

                var sut = fixture.Create<EnterspeedConnection>();

                var oldHttpClient = sut.HttpClientConnection;

                // Wait until timeout is hit
                Thread.Sleep(1001);

                var newHttpClient = sut.HttpClientConnection;

                Assert.NotEqual(oldHttpClient, newHttpClient);
            }
        }

        [Fact]
        public void ConnectionContainsVersion_Equal()
        {
            var fixture = new EnterspeedConnectionTestFixture();

            var enterspeedConfiguration = new EnterspeedConfiguration()
            {
                ApiKey = "source-" + Guid.NewGuid(),
                BaseUrl = "https://example.com/",
                ConnectionTimeout = 1
            };

            fixture.ConfigurationProvider
                .Configuration
                .Returns(enterspeedConfiguration);

            var sut = fixture.Create<EnterspeedConnection>();

            Assert.Equal("https://example.com/ingest/v1", sut.HttpClientConnection.BaseAddress.AbsoluteUri);
        }

        public class Flush
        {
            [Fact]
            public void CreatesNewHttpClient_NotEqual()
            {
                var fixture = new EnterspeedConnectionTestFixture();

                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration()
                    {
                        ApiKey = "source-" + Guid.NewGuid(),
                        BaseUrl = "https://example.com/"
                    });

                var sut = fixture.Create<EnterspeedConnection>();

                var oldHttpClient = sut.HttpClientConnection;

                sut.Flush();

                var newHttpClient = sut.HttpClientConnection;

                Assert.NotEqual(oldHttpClient, newHttpClient);
            }
        }
    }
}