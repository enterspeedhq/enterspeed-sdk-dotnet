using System.Collections.Generic;
using System.Net;
using Enterspeed.Source.Sdk.Domain.Connection;
using Enterspeed.Source.Sdk.Extensions;
using Xunit;

namespace Enterspeed.Source.Sdk.Tests.Extensions
{
    public class BulkResponseExtensionsTests
    {
        public class BulkIngestResponseGetSummary
        {
            [Fact]
            public void WithNullResponse_ReturnsNoResponse()
            {
                // Arrange
                BulkIngestResponse response = null;

                // Act
                var summary = response.GetSummary();

                // Assert
                Assert.Equal("No response", summary);
            }

            [Fact]
            public void WithFullSuccess_ReturnsCorrectSummary()
            {
                // Arrange
                var response = new BulkIngestResponse
                {
                    Success = true,
                    ChangedSourceEntities = new List<string> { "id1", "id2" },
                    UnchangedSourceEntities = new List<string> { "id3" },
                    Errors = new Dictionary<string, List<string>>()
                };

                // Act
                var summary = response.GetSummary();

                // Assert
                Assert.Contains("Success: True", summary);
                Assert.Contains("Changed: 2", summary);
                Assert.Contains("Unchanged: 1", summary);
                Assert.Contains("Errors: 0", summary);
                Assert.Contains("Result: Full Success", summary);
            }

            [Fact]
            public void WithPartialSuccess_ReturnsCorrectSummary()
            {
                // Arrange
                var response = new BulkIngestResponse
                {
                    Success = true,
                    ChangedSourceEntities = new List<string> { "id1" },
                    UnchangedSourceEntities = new List<string>(),
                    Errors = new Dictionary<string, List<string>>
                    {
                        ["id2"] = new List<string> { "Error" }
                    }
                };

                // Act
                var summary = response.GetSummary();

                // Assert
                Assert.Contains("Success: True", summary);
                Assert.Contains("Changed: 1", summary);
                Assert.Contains("Unchanged: 0", summary);
                Assert.Contains("Errors: 1", summary);
                Assert.Contains("Result: Partial Success", summary);
            }

            [Fact]
            public void WithFullFailure_ReturnsCorrectSummary()
            {
                // Arrange
                var response = new BulkIngestResponse
                {
                    Success = true,
                    ChangedSourceEntities = new List<string>(),
                    UnchangedSourceEntities = new List<string>(),
                    Errors = new Dictionary<string, List<string>>
                    {
                        ["id1"] = new List<string> { "Error 1" },
                        ["id2"] = new List<string> { "Error 2" }
                    }
                };

                // Act
                var summary = response.GetSummary();

                // Assert
                Assert.Contains("Success: True", summary);
                Assert.Contains("Changed: 0", summary);
                Assert.Contains("Unchanged: 0", summary);
                Assert.Contains("Errors: 2", summary);
                Assert.Contains("Result: Full Failure", summary);
            }
        }

        public class BulkDeleteResponseGetSummary
        {
            [Fact]
            public void WithNullResponse_ReturnsNoResponse()
            {
                // Arrange
                BulkDeleteResponse response = null;

                // Act
                var summary = response.GetSummary();

                // Assert
                Assert.Equal("No response", summary);
            }

            [Fact]
            public void WithFullSuccess_ReturnsCorrectSummary()
            {
                // Arrange
                var response = new BulkDeleteResponse
                {
                    Success = true,
                    DeletedSourceEntities = new List<string> { "id1", "id2" },
                    NotFoundSourceEntities = new List<string> { "id3" },
                    Errors = new Dictionary<string, List<string>>()
                };

                // Act
                var summary = response.GetSummary();

                // Assert
                Assert.Contains("Success: True", summary);
                Assert.Contains("Deleted: 2", summary);
                Assert.Contains("Not Found: 1", summary);
                Assert.Contains("Errors: 0", summary);
                Assert.Contains("Result: Full Success", summary);
            }

            [Fact]
            public void WithPartialSuccess_ReturnsCorrectSummary()
            {
                // Arrange
                var response = new BulkDeleteResponse
                {
                    Success = true,
                    DeletedSourceEntities = new List<string> { "id1" },
                    NotFoundSourceEntities = new List<string>(),
                    Errors = new Dictionary<string, List<string>>
                    {
                        ["id2"] = new List<string> { "Error" }
                    }
                };

                // Act
                var summary = response.GetSummary();

                // Assert
                Assert.Contains("Success: True", summary);
                Assert.Contains("Deleted: 1", summary);
                Assert.Contains("Not Found: 0", summary);
                Assert.Contains("Errors: 1", summary);
                Assert.Contains("Result: Partial Success", summary);
            }
        }

        public class BulkIngestResponseGetErrorSummary
        {
            [Fact]
            public void WithNullResponse_ReturnsNoErrors()
            {
                // Arrange
                BulkIngestResponse response = null;

                // Act
                var errorSummary = response.GetErrorSummary();

                // Assert
                Assert.Equal("No errors", errorSummary);
            }

            [Fact]
            public void WithNoErrors_ReturnsNoErrors()
            {
                // Arrange
                var response = new BulkIngestResponse
                {
                    Errors = new Dictionary<string, List<string>>()
                };

                // Act
                var errorSummary = response.GetErrorSummary();

                // Assert
                Assert.Equal("No errors", errorSummary);
            }

            [Fact]
            public void WithSingleError_ReturnsFormattedError()
            {
                // Arrange
                var response = new BulkIngestResponse
                {
                    Errors = new Dictionary<string, List<string>>
                    {
                        ["product-123"] = new List<string> { "Type is required" }
                    }
                };

                // Act
                var errorSummary = response.GetErrorSummary();

                // Assert
                Assert.Contains("product-123:", errorSummary);
                Assert.Contains("- Type is required", errorSummary);
            }

            [Fact]
            public void WithMultipleErrorsPerEntity_ReturnsAllErrors()
            {
                // Arrange
                var response = new BulkIngestResponse
                {
                    Errors = new Dictionary<string, List<string>>
                    {
                        ["product-123"] = new List<string> 
                        { 
                            "Type is required",
                            "OriginId is required"
                        }
                    }
                };

                // Act
                var errorSummary = response.GetErrorSummary();

                // Assert
                Assert.Contains("product-123:", errorSummary);
                Assert.Contains("- Type is required", errorSummary);
                Assert.Contains("- OriginId is required", errorSummary);
            }

            [Fact]
            public void WithMultipleEntities_ReturnsAllEntityErrors()
            {
                // Arrange
                var response = new BulkIngestResponse
                {
                    Errors = new Dictionary<string, List<string>>
                    {
                        ["product-123"] = new List<string> { "Error 1" },
                        ["product-456"] = new List<string> { "Error 2" },
                        ["entities[5]"] = new List<string> { "Error 3" }
                    }
                };

                // Act
                var errorSummary = response.GetErrorSummary();

                // Assert
                Assert.Contains("product-123:", errorSummary);
                Assert.Contains("product-456:", errorSummary);
                Assert.Contains("entities[5]:", errorSummary);
                Assert.Contains("- Error 1", errorSummary);
                Assert.Contains("- Error 2", errorSummary);
                Assert.Contains("- Error 3", errorSummary);
            }
        }

        public class BulkDeleteResponseGetErrorSummary
        {
            [Fact]
            public void WithNullResponse_ReturnsNoErrors()
            {
                // Arrange
                BulkDeleteResponse response = null;

                // Act
                var errorSummary = response.GetErrorSummary();

                // Assert
                Assert.Equal("No errors", errorSummary);
            }

            [Fact]
            public void WithNoErrors_ReturnsNoErrors()
            {
                // Arrange
                var response = new BulkDeleteResponse
                {
                    Errors = new Dictionary<string, List<string>>()
                };

                // Act
                var errorSummary = response.GetErrorSummary();

                // Assert
                Assert.Equal("No errors", errorSummary);
            }

            [Fact]
            public void WithErrors_ReturnsFormattedErrors()
            {
                // Arrange
                var response = new BulkDeleteResponse
                {
                    Errors = new Dictionary<string, List<string>>
                    {
                        ["originIds[2]"] = new List<string> { "OriginId cannot be empty" },
                        ["null"] = new List<string> { "OriginId cannot be null" }
                    }
                };

                // Act
                var errorSummary = response.GetErrorSummary();

                // Assert
                Assert.Contains("originIds[2]:", errorSummary);
                Assert.Contains("null:", errorSummary);
                Assert.Contains("- OriginId cannot be empty", errorSummary);
                Assert.Contains("- OriginId cannot be null", errorSummary);
            }
        }
    }
}

