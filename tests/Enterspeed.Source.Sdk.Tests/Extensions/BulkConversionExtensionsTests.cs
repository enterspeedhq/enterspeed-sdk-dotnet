using System.Collections.Generic;
using System.Linq;
using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Models.Bulk;
using Enterspeed.Source.Sdk.Extensions;
using Xunit;

namespace Enterspeed.Source.Sdk.Tests.Extensions
{
    public class BulkConversionExtensionsTests
    {
        public class ToBulkIngestEntity
        {
            [Fact]
            public void WithNullEntity_ReturnsNull()
            {
                // Arrange
                IEnterspeedEntity entity = null;

                // Act
                var result = entity.ToBulkIngestEntity();

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public void WithValidEntity_ConvertsCorrectly()
            {
                // Arrange
                var properties = new Dictionary<string, object>
                {
                    ["name"] = "Test Product",
                    ["price"] = 99.99
                };

                var entity = new EnterspeedEntity("product-123", "product", properties)
                {
                    Url = "/products/test",
                    ParentId = "category-456",
                    Redirects = new[] { "/old-url" }
                };

                // Act
                var result = entity.ToBulkIngestEntity();

                // Assert
                Assert.NotNull(result);
                Assert.Equal("product-123", result.OriginId);
                Assert.Equal("product", result.Type);
                Assert.Equal("/products/test", result.Url);
                Assert.Equal("category-456", result.OriginParentId);
                Assert.NotNull(result.Redirects);
                Assert.Single(result.Redirects);
                Assert.Equal("/old-url", result.Redirects[0]);
                Assert.NotNull(result.Properties);
            }

            [Fact]
            public void WithDictionaryProperties_MapsDirectly()
            {
                // Arrange
                var properties = new Dictionary<string, object>
                {
                    ["name"] = "Test",
                    ["count"] = 42
                };

                var entity = new EnterspeedEntity("test-1", "test", properties);

                // Act
                var result = entity.ToBulkIngestEntity();

                // Assert
                Assert.Same(properties, result.Properties);
                Assert.Equal("Test", result.Properties["name"]);
                Assert.Equal(42, result.Properties["count"]);
            }

            [Fact]
            public void WithNullProperties_HandlesGracefully()
            {
                // Arrange
                var entity = new EnterspeedEntity("test-1", "test", null);

                // Act
                var result = entity.ToBulkIngestEntity();

                // Assert
                Assert.NotNull(result);
                Assert.Null(result.Properties);
            }

            [Fact]
            public void WithNonDictionaryProperties_WrapsInContainer()
            {
                // Arrange
                var customObject = new { Name = "Test", Count = 42 };
                var entity = new EnterspeedEntity("test-1", "test", customObject);

                // Act
                var result = entity.ToBulkIngestEntity();

                // Assert
                Assert.NotNull(result);
                Assert.NotNull(result.Properties);
                Assert.True(result.Properties.ContainsKey("data"));
                Assert.Same(customObject, result.Properties["data"]);
            }
        }

        public class ToBulkIngestEntityGeneric
        {
            [Fact]
            public void WithNullEntity_ReturnsNull()
            {
                // Arrange
                IEnterspeedEntity<Dictionary<string, object>> entity = null;

                // Act
                var result = entity.ToBulkIngestEntity();

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public void WithGenericEntity_ConvertsCorrectly()
            {
                // Arrange
                var properties = new Dictionary<string, object>
                {
                    ["name"] = "Test Product"
                };

                var entity = new EnterspeedEntity<Dictionary<string, object>>("product-123", "product", properties);

                // Act
                var result = entity.ToBulkIngestEntity();

                // Assert
                Assert.NotNull(result);
                Assert.Equal("product-123", result.OriginId);
                Assert.Equal("product", result.Type);
                Assert.Same(properties, result.Properties);
            }

            [Fact]
            public void WithCustomTypeProperties_WrapsInContainer()
            {
                // Arrange
                var customProps = new { Name = "Test", Price = 99.99 };
                var entity = new EnterspeedEntity<object>("test-1", "test", customProps);

                // Act
                var result = entity.ToBulkIngestEntity();

                // Assert
                Assert.NotNull(result);
                Assert.NotNull(result.Properties);
                Assert.True(result.Properties.ContainsKey("data"));
                Assert.Same(customProps, result.Properties["data"]);
            }
        }

        public class ToBulkIngestEntities
        {
            [Fact]
            public void WithNullCollection_ReturnsEmptyEnumerable()
            {
                // Arrange
                IEnumerable<IEnterspeedEntity> entities = null;

                // Act
                var result = entities.ToBulkIngestEntities();

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }

            [Fact]
            public void WithEmptyCollection_ReturnsEmptyEnumerable()
            {
                // Arrange
                var entities = new List<IEnterspeedEntity>();

                // Act
                var result = entities.ToBulkIngestEntities();

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }

            [Fact]
            public void WithMultipleEntities_ConvertsAll()
            {
                // Arrange
                var entities = new List<IEnterspeedEntity>
                {
                    new EnterspeedEntity("product-1", "product", new Dictionary<string, object> { ["name"] = "Product 1" }),
                    new EnterspeedEntity("product-2", "product", new Dictionary<string, object> { ["name"] = "Product 2" }),
                    new EnterspeedEntity("product-3", "product", new Dictionary<string, object> { ["name"] = "Product 3" })
                };

                // Act
                var result = entities.ToBulkIngestEntities().ToList();

                // Assert
                Assert.Equal(3, result.Count);
                Assert.Equal("product-1", result[0].OriginId);
                Assert.Equal("product-2", result[1].OriginId);
                Assert.Equal("product-3", result[2].OriginId);
            }

            [Fact]
            public void WithMixedEntities_ConvertsAllCorrectly()
            {
                // Arrange
                var entities = new List<IEnterspeedEntity>
                {
                    new EnterspeedEntity("product-1", "product", new Dictionary<string, object>())
                    {
                        Url = "/products/1"
                    },
                    new EnterspeedEntity("category-1", "category", new Dictionary<string, object>())
                };

                // Act
                var result = entities.ToBulkIngestEntities().ToList();

                // Assert
                Assert.Equal(2, result.Count);
                Assert.Equal("product", result[0].Type);
                Assert.Equal("/products/1", result[0].Url);
                Assert.Equal("category", result[1].Type);
                Assert.Null(result[1].Url);
            }
        }
    }
}

