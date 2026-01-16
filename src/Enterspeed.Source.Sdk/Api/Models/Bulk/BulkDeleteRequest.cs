using System.Collections.Generic;

namespace Enterspeed.Source.Sdk.Api.Models.Bulk
{
    /// <summary>
    /// Request model for bulk delete operations.
    /// </summary>
    public class BulkDeleteRequest
    {
        /// <summary>
        /// Array of origin IDs to delete.
        /// </summary>
        public List<string> OriginIds { get; set; }

        public BulkDeleteRequest()
        {
            OriginIds = new List<string>();
        }

        public BulkDeleteRequest(IEnumerable<string> originIds)
        {
            OriginIds = new List<string>(originIds);
        }
    }
}

