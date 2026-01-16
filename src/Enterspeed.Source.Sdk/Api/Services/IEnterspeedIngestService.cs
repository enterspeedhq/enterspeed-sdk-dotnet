﻿using System.Collections.Generic;
using Enterspeed.Source.Sdk.Api.Connection;
using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Models.Bulk;
using Enterspeed.Source.Sdk.Domain.Connection;

namespace Enterspeed.Source.Sdk.Api.Services
{
    public interface IEnterspeedIngestService
    {
        Response Save(IEnterspeedEntity entity);
        Response Save(IEnterspeedEntity entity, IEnterspeedConnection connection);
        Response Save<T>(IEnterspeedEntity<T> entity);
        Response Save<T>(IEnterspeedEntity<T> entity, IEnterspeedConnection connection);    
        Response Delete(string id);
        Response Delete(string id, IEnterspeedConnection connection);

        /// <summary>
        /// Bulk ingest multiple entities in a single request.
        /// </summary>
        /// <param name="entities">Collection of entities to ingest.</param>
        /// <returns>Response containing results for all entities.</returns>
        BulkIngestResponse SaveBulk(IEnumerable<BulkIngestEntity> entities);

        /// <summary>
        /// Bulk ingest multiple entities in a single request using a custom connection.
        /// </summary>
        /// <param name="entities">Collection of entities to ingest.</param>
        /// <param name="connection">Custom connection to use.</param>
        /// <returns>Response containing results for all entities.</returns>
        BulkIngestResponse SaveBulk(IEnumerable<BulkIngestEntity> entities, IEnterspeedConnection connection);

        /// <summary>
        /// Bulk delete multiple entities by their origin IDs.
        /// </summary>
        /// <param name="originIds">Collection of origin IDs to delete.</param>
        /// <returns>Response containing results for all deletions.</returns>
        BulkDeleteResponse DeleteBulk(IEnumerable<string> originIds);

        /// <summary>
        /// Bulk delete multiple entities by their origin IDs using a custom connection.
        /// </summary>
        /// <param name="originIds">Collection of origin IDs to delete.</param>
        /// <param name="connection">Custom connection to use.</param>
        /// <returns>Response containing results for all deletions.</returns>
        BulkDeleteResponse DeleteBulk(IEnumerable<string> originIds, IEnterspeedConnection connection);

        /// <summary>
        /// Bulk delete multiple entities using a BulkDeleteRequest.
        /// </summary>
        /// <param name="request">The bulk delete request.</param>
        /// <returns>Response containing results for all deletions.</returns>
        BulkDeleteResponse DeleteBulk(BulkDeleteRequest request);

        /// <summary>
        /// Bulk delete multiple entities using a BulkDeleteRequest and custom connection.
        /// </summary>
        /// <param name="request">The bulk delete request.</param>
        /// <param name="connection">Custom connection to use.</param>
        /// <returns>Response containing results for all deletions.</returns>
        BulkDeleteResponse DeleteBulk(BulkDeleteRequest request, IEnterspeedConnection connection);
    }
}
