using System;
using System.Net;
using System.Net.Http;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Enterspeed.Source.Sdk.Api.Connection;
using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Providers;
using Enterspeed.Source.Sdk.Domain.Services;
using Enterspeed.Source.Sdk.Tests.Mock;
using NSubstitute;
using Xunit;

namespace Enterspeed.Source.Sdk.Tests.Domain.Services
{
    public class EnterspeedIngestServiceTests
    {
        public class EnterspeedIngestServiceTestFixture : Fixture
        {
            public IEnterspeedConnection Connection { get; set; }
            public IEnterspeedConfigurationProvider ConfigurationProvider { get; set; }
            public EnterspeedIngestServiceTestFixture()
            {
                Customize(new AutoNSubstituteCustomization());
                Connection = this.Freeze<IEnterspeedConnection>();
                ConfigurationProvider = this.Freeze<IEnterspeedConfigurationProvider>();
            }
        }

        public class Save
        {
            [Fact]
            public void HappyFlow_True()
            {
                var fixture = new EnterspeedIngestServiceTestFixture();

                var mockMessageHandler = new MockHttpMessageHandler("{\"status\": \"200\", \"message\": \"Entity Saved\"}", HttpStatusCode.OK);

                fixture.Connection
                    .HttpClientConnection
                    .Returns(new HttpClient(mockMessageHandler)
                    {
                        BaseAddress = new Uri("https://example.com/api")
                    });

                var mockEntity = Substitute.For<IEnterspeedEntity>();

                var sut = fixture.Create<EnterspeedIngestService>();

                var result = sut.Save(mockEntity);

                Assert.True(result.Success);
                Assert.Equal(200, result.StatusCode);
                Assert.Equal("Entity Saved", result.Message);
            }

            [Fact]
            public void MissingEntity_False()
            {
                var fixture = new EnterspeedIngestServiceTestFixture();

                var mockMessageHandler = new MockHttpMessageHandler(HttpStatusCode.OK);

                fixture.Connection
                    .HttpClientConnection
                    .Returns(new HttpClient(mockMessageHandler)
                    {
                        BaseAddress = new Uri("https://example.com")
                    });

                var sut = fixture.Create<EnterspeedIngestService>();

                var result = sut.Save(null);

                Assert.False(result.Success);
            }

            [Fact]
            public void EndpointError_False()
            {
                var fixture = new EnterspeedIngestServiceTestFixture();

                var mockMessageHandler = new MockHttpMessageHandler(HttpStatusCode.BadRequest, true);

                fixture.Connection
                    .HttpClientConnection
                    .Returns(new HttpClient(mockMessageHandler)
                    {
                        BaseAddress = new Uri("https://example.com")
                    });

                var sut = fixture.Create<EnterspeedIngestService>();

                var result = sut.Save(Substitute.For<IEnterspeedEntity>());

                Assert.False(result.Success);
            }
        }

        public class Delete
        {
            [Fact]
            public void HappyFlow_True()
            {
                var fixture = new EnterspeedIngestServiceTestFixture();

                var mockMessageHandler = new MockHttpMessageHandler(HttpStatusCode.OK);

                fixture.Connection
                    .HttpClientConnection
                    .Returns(new HttpClient(mockMessageHandler)
                    {
                        BaseAddress = new Uri("https://example.com")
                    });

                var sut = fixture.Create<EnterspeedIngestService>();

                var result = sut.Delete("1234");

                Assert.True(result.Success);
            }

            [Fact]
            public void MissingId_False()
            {
                var fixture = new EnterspeedIngestServiceTestFixture();

                var mockMessageHandler = new MockHttpMessageHandler("{\"status\": \"200\", \"message\": \"Entity Deleted\"}", HttpStatusCode.OK);

                fixture.Connection
                    .HttpClientConnection
                    .Returns(new HttpClient(mockMessageHandler)
                    {
                        BaseAddress = new Uri("https://example.com")
                    });

                var sut = fixture.Create<EnterspeedIngestService>();

                var result = sut.Delete(string.Empty);

                Assert.False(result.Success);
            }

            [Fact]
            public void EndpointError_False()
            {
                var fixture = new EnterspeedIngestServiceTestFixture();

                var mockMessageHandler = new MockHttpMessageHandler(HttpStatusCode.BadRequest, true);

                fixture.Connection
                    .HttpClientConnection
                    .Returns(new HttpClient(mockMessageHandler)
                    {
                        BaseAddress = new Uri("https://example.com")
                    });

                var sut = fixture.Create<EnterspeedIngestService>();

                var result = sut.Delete("1234");

                Assert.False(result.Success);
            }
        }

        public class Test
        {
            [Fact]
            public void HappyFlow_True()
            {
                var fixture = new EnterspeedIngestServiceTestFixture();

                var mockMessageHandler = new MockHttpMessageHandler(HttpStatusCode.OK);

                fixture.Connection
                    .HttpClientConnection
                    .Returns(new HttpClient(mockMessageHandler)
                    {
                        BaseAddress = new Uri("https://example.com")
                    });

                var sut = fixture.Create<EnterspeedIngestService>();

                var result = sut.Test();

                Assert.True(result.Success);
            }

            [Fact]
            public void EndpointError_False()
            {
                var fixture = new EnterspeedIngestServiceTestFixture();

                var mockMessageHandler = new MockHttpMessageHandler(HttpStatusCode.BadRequest, true);

                fixture.Connection
                    .HttpClientConnection
                    .Returns(new HttpClient(mockMessageHandler)
                    {
                        BaseAddress = new Uri("https://example.com")
                    });

                var sut = fixture.Create<EnterspeedIngestService>();

                var result = sut.Test();

                Assert.False(result.Success);
            }
        }
    }
}