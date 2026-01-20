using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Enterspeed.Source.Sdk.Domain.Connection
{
    /// <summary>
    /// Response from a bulk ingest operation.
    /// </summary>
    public class BulkIngestResponse
    {
        /// <summary>
        /// List of origin IDs that were successfully ingested and had changes.
        /// </summary>
        public List<string> ChangedSourceEntities { get; set; }

        /// <summary>
        /// List of origin IDs that were processed but had no changes.
        /// </summary>
        public List<string> UnchangedSourceEntities { get; set; }

        /// <summary>
        /// Dictionary of errors keyed by origin ID (or entities[index] for invalid entities).
        /// Each value is a list of error messages for that entity.
        /// </summary>
        public Dictionary<string, List<string>> Errors { get; set; }

        /// <summary>
        /// HTTP status code from the response.
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// Exception that occurred during the request, if any.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Raw response content from the API.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Indicates whether the HTTP request was successful (2xx status code).
        /// </summary>
        public bool Success { get; set; }

        public BulkIngestResponse()
        {
            ChangedSourceEntities = new List<string>();
            UnchangedSourceEntities = new List<string>();
            Errors = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// Returns true if all entities were processed successfully without errors.
        /// </summary>
        public bool IsFullSuccess => Success && (Errors == null || Errors.Count == 0);

        /// <summary>
        /// Returns true if some entities succeeded and some failed.
        /// </summary>
        public bool IsPartialSuccess =>
            Success &&
            (ChangedSourceEntities.Count + UnchangedSourceEntities.Count) > 0 &&
            Errors != null &&
            Errors.Count > 0;

        /// <summary>
        /// Returns true if all entities failed or the request failed.
        /// </summary>
        public bool IsFullFailure =>
            !Success ||
            (ChangedSourceEntities.Count == 0 &&
             UnchangedSourceEntities.Count == 0 &&
             Errors != null &&
             Errors.Count > 0);

        /// <summary>
        /// Gets the total number of successfully processed entities (changed + unchanged).
        /// </summary>
        public int SuccessCount => ChangedSourceEntities.Count + UnchangedSourceEntities.Count;

        /// <summary>
        /// Gets the total number of failed entities.
        /// </summary>
        public int ErrorCount => Errors?.Count ?? 0;

        /// <summary>
        /// Gets all origin IDs that had errors.
        /// </summary>
        public IEnumerable<string> FailedOriginIds => Errors?.Keys ?? Enumerable.Empty<string>();

        /// <summary>
        /// Gets all error messages for a specific origin ID.
        /// </summary>
        public IEnumerable<string> GetErrorsForOriginId(string originId)
        {
            if (Errors != null && Errors.TryGetValue(originId, out var errors))
            {
                return errors;
            }
            return Enumerable.Empty<string>();
        }
    }
}

