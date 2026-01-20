using System.Collections.Generic;
using System.Linq;
using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Models.Bulk;

namespace Enterspeed.Source.Sdk.Extensions
{
    /// <summary>
    /// Extension methods for converting between single and bulk entity formats.
    /// </summary>
    public static class BulkConversionExtensions
    {
        /// <summary>
        /// Converts an IEnterspeedEntity to a BulkIngestEntity.
        /// </summary>
        public static BulkIngestEntity ToBulkIngestEntity(this IEnterspeedEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            // Handle Properties based on its actual type
            IDictionary<string, object> properties = null;
            if (entity.Properties != null)
            {
                if (entity.Properties is IDictionary<string, object> dict)
                {
                    properties = dict;
                }
                else
                {
                    // For other types, wrap in a container that will be serialized
                    properties = new Dictionary<string, object>
                    {
                        ["data"] = entity.Properties
                    };
                }
            }

            return new BulkIngestEntity
            {
                OriginId = entity.Id,
                Type = entity.Type,
                Url = entity.Url,
                OriginParentId = entity.ParentId,
                Properties = properties,
                Redirects = entity.Redirects
            };
        }

        /// <summary>
        /// Converts a collection of IEnterspeedEntity to BulkIngestEntity collection.
        /// </summary>
        public static IEnumerable<BulkIngestEntity> ToBulkIngestEntities(
            this IEnumerable<IEnterspeedEntity> entities)
        {
            return entities?.Select(e => e.ToBulkIngestEntity()) ?? Enumerable.Empty<BulkIngestEntity>();
        }

        /// <summary>
        /// Converts a generic IEnterspeedEntity to a BulkIngestEntity.
        /// </summary>
        public static BulkIngestEntity ToBulkIngestEntity<T>(this IEnterspeedEntity<T> entity)
        {
            if (entity == null)
            {
                return null;
            }

            // For generic entities, Properties is of type T
            IDictionary<string, object> properties = null;
            if (entity.Properties != null)
            {
                if (entity.Properties is IDictionary<string, object> dict)
                {
                    properties = dict;
                }
                else
                {
                    // For other types, wrap in a container for serialization
                    properties = new Dictionary<string, object>
                    {
                        ["data"] = entity.Properties
                    };
                }
            }

            return new BulkIngestEntity
            {
                OriginId = entity.Id,
                Type = entity.Type,
                Url = entity.Url,
                OriginParentId = entity.ParentId,
                Properties = properties,
                Redirects = entity.Redirects
            };
        }
    }
}

