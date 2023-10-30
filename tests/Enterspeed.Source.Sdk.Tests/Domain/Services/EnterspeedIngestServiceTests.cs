using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Enterspeed.Source.Sdk.Api.Connection;
using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Models.Properties;
using Enterspeed.Source.Sdk.Api.Providers;
using Enterspeed.Source.Sdk.Api.Services;
using Enterspeed.Source.Sdk.Configuration;
using Enterspeed.Source.Sdk.Domain.Services;
using Enterspeed.Source.Sdk.Domain.SystemTextJson;
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

                this.Register<IJsonSerializer>(() => new SystemTextJsonSerializer());
            }
        }

        public class Save
        {
            [Fact]
            public void HappyFlow_True()
            {
                var fixture = new EnterspeedIngestServiceTestFixture();

                var responseContent = "{\"status\": 200, \"message\": \"Entity Saved\"}";
                var mockMessageHandler = new MockHttpMessageHandler(
                    responseContent, HttpStatusCode.OK);

                fixture.Connection
                    .HttpClientConnection
                    .Returns(
                        new HttpClient(mockMessageHandler)
                        {
                            BaseAddress = new Uri("https://example.com/")
                        });

                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration());

                var mockEntity = new MockEnterspeedEntity()
                {
                    Id = "1234",
                    Type = "test",
                    Properties = new Dictionary<string, IEnterspeedProperty>
                    {
                        ["testProperty"] = new StringEnterspeedProperty("title", "Hello world")
                    }
                };

                var sut = fixture.Create<EnterspeedIngestService>();

                var result = sut.Save(mockEntity);

                Assert.True(result.Success);
                Assert.Equal(200, result.StatusCode);
                Assert.Equal("Entity Saved", result.Message);
                Assert.Equal(responseContent, result.Content);
            }

            [Fact]
            public void EndpointError_False()
            {
                var fixture = new EnterspeedIngestServiceTestFixture();

                var mockMessageHandler = new MockHttpMessageHandler(HttpStatusCode.BadRequest, true);

                fixture.Connection
                    .HttpClientConnection
                    .Returns(
                        new HttpClient(mockMessageHandler)
                        {
                            BaseAddress = new Uri("https://example.com")
                        });

                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration());

                var sut = fixture.Create<EnterspeedIngestService>();

                var result = sut.Save(Substitute.For<IEnterspeedEntity>());

                Assert.False(result.Success);
            }

            [Fact]
            public void InvalidSourceApiKey_Error()
            {
                var fixture = new EnterspeedIngestServiceTestFixture();

                var mockMessageHandler = new MockHttpMessageHandler(null, HttpStatusCode.Unauthorized);

                fixture.Connection
                    .HttpClientConnection
                    .Returns(
                        new HttpClient(mockMessageHandler)
                        {
                            BaseAddress = new Uri("https://example.com/")
                        });

                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration());

                var sut = fixture.Create<EnterspeedIngestService>();

                var entity = Substitute.For<EnterspeedEntity>("id", "test-type", new object());

                var result = sut.Save(entity);

                Assert.False(result.Success);
                Assert.Equal(401, result.StatusCode);
            }

            [Fact]
            public void InvalidSourceEntityPropertyName_Error()
            {
                var fixture = new EnterspeedIngestServiceTestFixture();

                var mockMessageHandler = new MockHttpMessageHandler(
        @"{
                    ""errors"": {
                        ""properties.123"": ""Property name cannot start with a digit""
                    },
                    ""errorCode"": ""s-1001"",
                    ""status"": 422,
                    ""message"": ""Invalid request""
                }", HttpStatusCode.Unauthorized);

                fixture.Connection
                    .HttpClientConnection
                    .Returns(
                        new HttpClient(mockMessageHandler)
                        {
                            BaseAddress = new Uri("https://example.com/")
                        });

                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration());

                var sut = fixture.Create<EnterspeedIngestService>();

                var entity = Substitute.For<EnterspeedEntity>("id", "test-type", new object());

                var result = sut.Save(entity);

                Assert.False(result.Success);
                Assert.Equal(422, result.StatusCode);
                Assert.Equal("s-1001", result.ErrorCode);
                Assert.Equal("Property name cannot start with a digit", result.Errors["properties.123"]);
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
                    .Returns(
                        new HttpClient(mockMessageHandler)
                        {
                            BaseAddress = new Uri("https://example.com")
                        });

                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration());

                var sut = fixture.Create<EnterspeedIngestService>();

                var result = sut.Delete("1234");

                Assert.True(result.Success);
            }

            [Fact]
            public void MissingId_False()
            {
                var fixture = new EnterspeedIngestServiceTestFixture();

                var mockMessageHandler = new MockHttpMessageHandler(
                    "{\"status\": 200, \"message\": \"Entity Deleted\"}", HttpStatusCode.OK);

                fixture.Connection
                    .HttpClientConnection
                    .Returns(
                        new HttpClient(mockMessageHandler)
                        {
                            BaseAddress = new Uri("https://example.com")
                        });

                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration());

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
                    .Returns(
                        new HttpClient(mockMessageHandler)
                        {
                            BaseAddress = new Uri("https://example.com")
                        });

                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration());

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
                    .Returns(
                        new HttpClient(mockMessageHandler)
                        {
                            BaseAddress = new Uri("https://example.com")
                        });

                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration());

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
                    .Returns(
                        new HttpClient(mockMessageHandler)
                        {
                            BaseAddress = new Uri("https://example.com")
                        });

                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration());

                var sut = fixture.Create<EnterspeedIngestService>();

                var result = sut.Test();

                Assert.False(result.Success);
            }
        }
    }
}