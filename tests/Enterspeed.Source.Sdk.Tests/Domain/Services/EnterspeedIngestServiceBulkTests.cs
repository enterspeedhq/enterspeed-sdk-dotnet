using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Enterspeed.Source.Sdk.Api.Connection;
using Enterspeed.Source.Sdk.Api.Models.Bulk;
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
    public class EnterspeedIngestServiceBulkTests
    {
        public class BulkTestFixture : Fixture
        {
            public IEnterspeedConnection Connection { get; set; }
            public IEnterspeedConfigurationProvider ConfigurationProvider { get; set; }

            public BulkTestFixture()
            {
                Customize(new AutoNSubstituteCustomization());
                Connection = this.Freeze<IEnterspeedConnection>();
                ConfigurationProvider = this.Freeze<IEnterspeedConfigurationProvider>();

                this.Register<IJsonSerializer>(() => new SystemTextJsonSerializer());
            }
        }

        public class SaveBulk
        {
            [Fact]
            public void WithNullEntities_ReturnsFailureResponse()
            {
                // Arrange
                var fixture = new BulkTestFixture();
                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration());

                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.SaveBulk(null);

                // Assert
                Assert.False(response.Success);
                Assert.NotNull(response.Exception);
                Assert.IsType<ArgumentNullException>(response.Exception);
            }

            [Fact]
            public void WithEmptyList_ReturnsSuccessResponse()
            {
                // Arrange
                var fixture = new BulkTestFixture();
                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration());

                var entities = new List<BulkIngestEntity>();
                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.SaveBulk(entities);

                // Assert
                Assert.True(response.Success);
                Assert.Equal(HttpStatusCode.OK, response.Status);
                Assert.Empty(response.ChangedSourceEntities);
                Assert.Empty(response.UnchangedSourceEntities);
                Assert.Empty(response.Errors);
            }

            [Fact]
            public void WithValidEntities_AllChanged_ReturnsFullSuccess()
            {
                // Arrange
                var fixture = new BulkTestFixture();

                var responseContent = @"{
                    ""changedSourceEntities"": [""product-1"", ""product-2""],
                    ""unchangedSourceEntities"": [],
                    ""errors"": {}
                }";

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

                var entities = new List<BulkIngestEntity>
                {
                    new BulkIngestEntity("product-1", "product", new Dictionary<string, object>
                    {
                        ["name"] = "Product 1",
                        ["price"] = 99.99
                    }),
                    new BulkIngestEntity("product-2", "product", new Dictionary<string, object>
                    {
                        ["name"] = "Product 2",
                        ["price"] = 149.99
                    })
                };

                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.SaveBulk(entities);

                // Assert
                Assert.True(response.Success);
                Assert.True(response.IsFullSuccess);
                Assert.False(response.IsPartialSuccess);
                Assert.False(response.IsFullFailure);
                Assert.Equal(2, response.ChangedSourceEntities.Count);
                Assert.Contains("product-1", response.ChangedSourceEntities);
                Assert.Contains("product-2", response.ChangedSourceEntities);
                Assert.Empty(response.UnchangedSourceEntities);
                Assert.Empty(response.Errors);
                Assert.Equal(2, response.SuccessCount);
                Assert.Equal(0, response.ErrorCount);
            }

            [Fact]
            public void WithValidEntities_SomeUnchanged_ReturnsFullSuccess()
            {
                // Arrange
                var fixture = new BulkTestFixture();

                var responseContent = @"{
                    ""changedSourceEntities"": [""product-1""],
                    ""unchangedSourceEntities"": [""product-2""],
                    ""errors"": {}
                }";

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

                var entities = new List<BulkIngestEntity>
                {
                    new BulkIngestEntity("product-1", "product", new Dictionary<string, object>()),
                    new BulkIngestEntity("product-2", "product", new Dictionary<string, object>())
                };

                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.SaveBulk(entities);

                // Assert
                Assert.True(response.Success);
                Assert.True(response.IsFullSuccess);
                Assert.Single(response.ChangedSourceEntities);
                Assert.Single(response.UnchangedSourceEntities);
                Assert.Empty(response.Errors);
                Assert.Equal(2, response.SuccessCount);
            }

            [Fact]
            public void WithPartialFailure_ReturnsPartialSuccess()
            {
                // Arrange
                var fixture = new BulkTestFixture();

                var responseContent = @"{
                    ""changedSourceEntities"": [""product-1""],
                    ""unchangedSourceEntities"": [],
                    ""errors"": {
                        ""product-2"": [""Type is required and must not be empty or whitespace""],
                        ""entities[2]"": [""OriginId is required and must not be empty or whitespace""]
                    }
                }";

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

                var entities = new List<BulkIngestEntity>
                {
                    new BulkIngestEntity("product-1", "product", new Dictionary<string, object>()),
                    new BulkIngestEntity("product-2", null, new Dictionary<string, object>()),
                    new BulkIngestEntity(null, "product", new Dictionary<string, object>())
                };

                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.SaveBulk(entities);

                // Assert
                Assert.True(response.Success);
                Assert.False(response.IsFullSuccess);
                Assert.True(response.IsPartialSuccess);
                Assert.False(response.IsFullFailure);
                Assert.Single(response.ChangedSourceEntities);
                Assert.Equal(2, response.Errors.Count);
                Assert.True(response.Errors.ContainsKey("product-2"));
                Assert.True(response.Errors.ContainsKey("entities[2]"));
                Assert.Equal(1, response.SuccessCount);
                Assert.Equal(2, response.ErrorCount);
                Assert.Contains("product-2", response.FailedOriginIds);
                Assert.Contains("entities[2]", response.FailedOriginIds);
            }

            [Fact]
            public void WithAllFailures_ReturnsFullFailure()
            {
                // Arrange
                var fixture = new BulkTestFixture();

                var responseContent = @"{
                    ""changedSourceEntities"": [],
                    ""unchangedSourceEntities"": [],
                    ""errors"": {
                        ""product-1"": [""Type is required""],
                        ""product-2"": [""OriginId is required""]
                    }
                }";

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

                var entities = new List<BulkIngestEntity>
                {
                    new BulkIngestEntity("product-1", null, new Dictionary<string, object>()),
                    new BulkIngestEntity(null, "product", new Dictionary<string, object>())
                };

                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.SaveBulk(entities);

                // Assert
                Assert.True(response.Success);
                Assert.False(response.IsFullSuccess);
                Assert.False(response.IsPartialSuccess);
                Assert.True(response.IsFullFailure);
                Assert.Empty(response.ChangedSourceEntities);
                Assert.Empty(response.UnchangedSourceEntities);
                Assert.Equal(2, response.Errors.Count);
                Assert.Equal(0, response.SuccessCount);
                Assert.Equal(2, response.ErrorCount);
            }

            [Fact]
            public void GetErrorsForOriginId_ReturnsCorrectErrors()
            {
                // Arrange
                var fixture = new BulkTestFixture();

                var responseContent = @"{
                    ""changedSourceEntities"": [],
                    ""unchangedSourceEntities"": [],
                    ""errors"": {
                        ""product-1"": [""Error 1"", ""Error 2""],
                        ""product-2"": [""Error 3""]
                    }
                }";

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

                var entities = new List<BulkIngestEntity>
                {
                    new BulkIngestEntity("product-1", "product", new Dictionary<string, object>())
                };

                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.SaveBulk(entities);
                var errors = response.GetErrorsForOriginId("product-1").ToList();

                // Assert
                Assert.Equal(2, errors.Count);
                Assert.Contains("Error 1", errors);
                Assert.Contains("Error 2", errors);
            }

            [Fact]
            public void WithHttpException_ReturnsFailureResponse()
            {
                // Arrange
                var fixture = new BulkTestFixture();

                var mockMessageHandler = new MockHttpMessageHandler(HttpStatusCode.BadRequest, throwException: true);

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

                var entities = new List<BulkIngestEntity>
                {
                    new BulkIngestEntity("product-1", "product", new Dictionary<string, object>())
                };

                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.SaveBulk(entities);

                // Assert
                Assert.False(response.Success);
                Assert.NotNull(response.Exception);
            }

            [Fact]
            public void WithFeatureNotEnabled_ReturnsErrorResponse()
            {
                // Arrange
                var fixture = new BulkTestFixture();

                var responseContent = @"{
                    ""error"": ""Bulk ingest not available for this tenant""
                }";

                var mockMessageHandler = new MockHttpMessageHandler(
                    responseContent, HttpStatusCode.BadRequest);

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

                var entities = new List<BulkIngestEntity>
                {
                    new BulkIngestEntity("product-1", "product", new Dictionary<string, object>())
                };

                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.SaveBulk(entities);

                // Assert
                Assert.False(response.Success);
                Assert.Equal(HttpStatusCode.BadRequest, response.Status);
            }
        }

        public class DeleteBulk
        {
            [Fact]
            public void WithNullOriginIds_ReturnsFailureResponse()
            {
                // Arrange
                var fixture = new BulkTestFixture();
                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration());

                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.DeleteBulk((IEnumerable<string>)null);

                // Assert
                Assert.False(response.Success);
                Assert.NotNull(response.Exception);
                Assert.IsType<ArgumentNullException>(response.Exception);
            }

            [Fact]
            public void WithNullRequest_ReturnsFailureResponse()
            {
                // Arrange
                var fixture = new BulkTestFixture();
                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration());

                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.DeleteBulk((BulkDeleteRequest)null);

                // Assert
                Assert.False(response.Success);
                Assert.NotNull(response.Exception);
                Assert.IsType<ArgumentNullException>(response.Exception);
            }

            [Fact]
            public void WithEmptyList_ReturnsSuccessResponse()
            {
                // Arrange
                var fixture = new BulkTestFixture();
                fixture.ConfigurationProvider
                    .Configuration
                    .Returns(new EnterspeedConfiguration());

                var originIds = new List<string>();
                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.DeleteBulk(originIds);

                // Assert
                Assert.True(response.Success);
                Assert.Equal(HttpStatusCode.OK, response.Status);
                Assert.Empty(response.DeletedSourceEntities);
                Assert.Empty(response.NotFoundSourceEntities);
                Assert.Empty(response.Errors);
            }

            [Fact]
            public void WithValidIds_AllDeleted_ReturnsFullSuccess()
            {
                // Arrange
                var fixture = new BulkTestFixture();

                var responseContent = @"{
                    ""deletedSourceEntities"": [""product-1"", ""product-2""],
                    ""notFoundSourceEntities"": [],
                    ""errors"": {}
                }";

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

                var originIds = new[] { "product-1", "product-2" };
                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.DeleteBulk(originIds);

                // Assert
                Assert.True(response.Success);
                Assert.True(response.IsFullSuccess);
                Assert.Equal(2, response.DeletedSourceEntities.Count);
                Assert.Contains("product-1", response.DeletedSourceEntities);
                Assert.Contains("product-2", response.DeletedSourceEntities);
                Assert.Empty(response.NotFoundSourceEntities);
                Assert.Empty(response.Errors);
                Assert.Equal(2, response.SuccessCount);
                Assert.Equal(0, response.ErrorCount);
            }

            [Fact]
            public void WithSomeNotFound_ReturnsFullSuccess()
            {
                // Arrange
                var fixture = new BulkTestFixture();

                var responseContent = @"{
                    ""deletedSourceEntities"": [""product-1""],
                    ""notFoundSourceEntities"": [""product-2"", ""product-3""],
                    ""errors"": {}
                }";

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

                var originIds = new[] { "product-1", "product-2", "product-3" };
                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.DeleteBulk(originIds);

                // Assert
                Assert.True(response.Success);
                Assert.True(response.IsFullSuccess);
                Assert.False(response.IsPartialSuccess);
                Assert.Single(response.DeletedSourceEntities);
                Assert.Equal(2, response.NotFoundSourceEntities.Count);
                Assert.Empty(response.Errors);
                Assert.Equal(3, response.SuccessCount); // NotFound is success
            }

            [Fact]
            public void WithPartialFailure_ReturnsPartialSuccess()
            {
                // Arrange
                var fixture = new BulkTestFixture();

                var responseContent = @"{
                    ""deletedSourceEntities"": [""product-1""],
                    ""notFoundSourceEntities"": [""product-2""],
                    ""errors"": {
                        ""originIds[2]"": [""OriginId cannot be empty at position 2""],
                        ""null"": [""OriginId cannot be null""]
                    }
                }";

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

                var originIds = new[] { "product-1", "product-2" };
                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.DeleteBulk(originIds);

                // Assert
                Assert.True(response.Success);
                Assert.False(response.IsFullSuccess);
                Assert.True(response.IsPartialSuccess);
                Assert.Single(response.DeletedSourceEntities);
                Assert.Single(response.NotFoundSourceEntities);
                Assert.Equal(2, response.Errors.Count);
                Assert.Equal(2, response.SuccessCount);
                Assert.Equal(2, response.ErrorCount);
            }

            [Fact]
            public void WithBulkDeleteRequest_WorksCorrectly()
            {
                // Arrange
                var fixture = new BulkTestFixture();

                var responseContent = @"{
                    ""deletedSourceEntities"": [""product-1""],
                    ""notFoundSourceEntities"": [],
                    ""errors"": {}
                }";

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

                var request = new BulkDeleteRequest(new[] { "product-1" });
                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.DeleteBulk(request);

                // Assert
                Assert.True(response.Success);
                Assert.True(response.IsFullSuccess);
                Assert.Single(response.DeletedSourceEntities);
            }

            [Fact]
            public void WithHttpException_ReturnsFailureResponse()
            {
                // Arrange
                var fixture = new BulkTestFixture();

                var mockMessageHandler = new MockHttpMessageHandler(HttpStatusCode.BadRequest, throwException: true);

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

                var originIds = new[] { "product-1" };
                var sut = fixture.Create<EnterspeedIngestService>();

                // Act
                var response = sut.DeleteBulk(originIds);

                // Assert
                Assert.False(response.Success);
                Assert.NotNull(response.Exception);
            }
        }
    }
}

