using System.Collections.Generic;

namespace Enterspeed.Source.Sdk.Api.Models.Bulk
{
    /// <summary>
    /// Represents an entity to be ingested in a bulk operation.
    /// Maps to the Enterspeed V2 bulk ingest API format.
    /// </summary>
    public class BulkIngestEntity
    {
        /// <summary>
        /// The unique identifier for the entity (OriginId in Enterspeed).
        /// </summary>
        public string OriginId { get; set; }

        /// <summary>
        /// The type of the entity (e.g., "product", "blogPage").
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The URL path for routable entities. Must start with '/' if provided.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The parent entity ID for hierarchical structures (OriginParentId in Enterspeed).
        /// </summary>
        public string OriginParentId { get; set; }

        /// <summary>
        /// The entity's data properties.
        /// </summary>
        public IDictionary<string, object> Properties { get; set; }

        /// <summary>
        /// Incoming redirects for the entity.
        /// </summary>
        public string[] Redirects { get; set; }

        public BulkIngestEntity()
        {
        }

        public BulkIngestEntity(string originId, string type, IDictionary<string, object> properties = null)
        {
            OriginId = originId;
            Type = type;
            Properties = properties ?? new Dictionary<string, object>();
        }
    }
}

