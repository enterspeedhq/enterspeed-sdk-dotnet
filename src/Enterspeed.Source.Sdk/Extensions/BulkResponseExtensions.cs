using System.Linq;
using System.Text;
using Enterspeed.Source.Sdk.Domain.Connection;

namespace Enterspeed.Source.Sdk.Extensions
{
    /// <summary>
    /// Extension methods for working with bulk operation responses.
    /// </summary>
    public static class BulkResponseExtensions
    {
        /// <summary>
        /// Gets a human-readable summary of the bulk ingest response.
        /// </summary>
        public static string GetSummary(this BulkIngestResponse response)
        {
            if (response == null)
            {
                return "No response";
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Success: {response.Success}");
            sb.AppendLine($"Changed: {response.ChangedSourceEntities.Count}");
            sb.AppendLine($"Unchanged: {response.UnchangedSourceEntities.Count}");
            sb.AppendLine($"Errors: {response.ErrorCount}");

            if (response.IsFullSuccess)
            {
                sb.AppendLine("Result: Full Success");
            }
            else if (response.IsPartialSuccess)
            {
                sb.AppendLine("Result: Partial Success");
            }
            else if (response.IsFullFailure)
            {
                sb.AppendLine("Result: Full Failure");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets a human-readable summary of the bulk delete response.
        /// </summary>
        public static string GetSummary(this BulkDeleteResponse response)
        {
            if (response == null)
            {
                return "No response";
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Success: {response.Success}");
            sb.AppendLine($"Deleted: {response.DeletedSourceEntities.Count}");
            sb.AppendLine($"Not Found: {response.NotFoundSourceEntities.Count}");
            sb.AppendLine($"Errors: {response.ErrorCount}");

            if (response.IsFullSuccess)
            {
                sb.AppendLine("Result: Full Success");
            }
            else if (response.IsPartialSuccess)
            {
                sb.AppendLine("Result: Partial Success");
            }
            else if (response.IsFullFailure)
            {
                sb.AppendLine("Result: Full Failure");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets all error messages as a formatted string.
        /// </summary>
        public static string GetErrorSummary(this BulkIngestResponse response)
        {
            if (response?.Errors == null || !response.Errors.Any())
            {
                return "No errors";
            }

            var sb = new StringBuilder();
            foreach (var error in response.Errors)
            {
                sb.AppendLine($"{error.Key}:");
                foreach (var message in error.Value)
                {
                    sb.AppendLine($"  - {message}");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets all error messages as a formatted string.
        /// </summary>
        public static string GetErrorSummary(this BulkDeleteResponse response)
        {
            if (response?.Errors == null || !response.Errors.Any())
            {
                return "No errors";
            }

            var sb = new StringBuilder();
            foreach (var error in response.Errors)
            {
                sb.AppendLine($"{error.Key}:");
                foreach (var message in error.Value)
                {
                    sb.AppendLine($"  - {message}");
                }
            }

            return sb.ToString();
        }
    }
}

