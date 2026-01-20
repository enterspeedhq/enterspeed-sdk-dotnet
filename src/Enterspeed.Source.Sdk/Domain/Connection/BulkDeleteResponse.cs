using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Enterspeed.Source.Sdk.Domain.Connection
{
    /// <summary>
    /// Response from a bulk delete operation.
    /// </summary>
    public class BulkDeleteResponse
    {
        /// <summary>
        /// List of origin IDs that were successfully deleted.
        /// </summary>
        public List<string> DeletedSourceEntities { get; set; }

        /// <summary>
        /// List of origin IDs that were not found (already deleted or never existed).
        /// This is not an error - indicates idempotent delete behavior.
        /// </summary>
        public List<string> NotFoundSourceEntities { get; set; }

        /// <summary>
        /// Dictionary of errors keyed by origin ID (or originIds[index] or special keys).
        /// Each value is a list of error messages for that origin ID.
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

        public BulkDeleteResponse()
        {
            DeletedSourceEntities = new List<string>();
            NotFoundSourceEntities = new List<string>();
            Errors = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// Returns true if all entities were processed successfully without errors.
        /// Note: NotFound entities are considered successful (idempotent delete).
        /// </summary>
        public bool IsFullSuccess => Success && (Errors == null || Errors.Count == 0);

        /// <summary>
        /// Returns true if some entities succeeded and some failed.
        /// </summary>
        public bool IsPartialSuccess =>
            Success &&
            (DeletedSourceEntities.Count + NotFoundSourceEntities.Count) > 0 &&
            Errors != null &&
            Errors.Count > 0;

        /// <summary>
        /// Returns true if all entities failed or the request failed.
        /// </summary>
        public bool IsFullFailure =>
            !Success ||
            (DeletedSourceEntities.Count == 0 &&
             NotFoundSourceEntities.Count == 0 &&
             Errors != null &&
             Errors.Count > 0);

        /// <summary>
        /// Gets the total number of successfully processed entities (deleted + not found).
        /// </summary>
        public int SuccessCount => DeletedSourceEntities.Count + NotFoundSourceEntities.Count;

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

