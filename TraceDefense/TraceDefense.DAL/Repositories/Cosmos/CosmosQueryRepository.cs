using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos;
using TraceDefense.DAL.Providers;
using TraceDefense.Entities;
using TraceDefense.Entities.Geospatial;

namespace TraceDefense.DAL.Repositories.Cosmos
{
    /// <summary>
    /// CosmosDB implementation of <see cref="IQueryRepository"/>
    /// </summary>
    public class CosmosQueryRepository : CosmosRepository, IQueryRepository
    {
        /// <summary>
        /// <see cref="Query"/> container object
        /// </summary>
        private Container _queryContainer;
        private Container _queryIdsContainer;
        /// <summary>
        /// Name of <see cref="Query"/> container
        /// </summary>
        public const string QUERIES_CONTAINER_NAME = "queries_test";

        public const string QUERYIDS_CONTAINER_NAME = "query_ids_test";

        /// <summary>
        /// Creates a new <see cref="CosmosQueryRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">CosmosDB connection factory instance</param>
        public CosmosQueryRepository(CosmosConnectionFactory connectionFactory) : base(connectionFactory)
        {
            // Create container reference
            this._queryContainer = this.Database.GetContainer(QUERIES_CONTAINER_NAME);

            this._queryIdsContainer = this.Database.GetContainer(QUERYIDS_CONTAINER_NAME);
        }

        /// <inheritdoc/>
        public async Task<IList<Query>> GetQueriesAsync(IList<string> queryIds, CancellationToken cancellationToken = default)
        {
            // Validate inputs
            if (queryIds == null || queryIds.Count() == 0)
            {
                throw new ArgumentNullException(nameof(queryIds));
            }

            // Get unique Query IDs from provided collection to build WHERE clause
            IEnumerable<string> queryIdsSanitized = queryIds.Select(q => String.Format("'{0}'", q));
            string whereIds = String.Join(",", queryIdsSanitized);

            // Build query
            string sqlQuery = String.Format("SELECT * FROM c WHERE c.id IN ({0})", whereIds);
            QueryDefinition cosmosQueryDef = new QueryDefinition(sqlQuery);

            // Get results
            FeedIterator<QueryRecord> resultIterator = this._queryContainer
                .GetItemQueryIterator<QueryRecord>(cosmosQueryDef);
            var queries = new List<Query>();

            while (resultIterator.HasMoreResults)
            {
                FeedResponse<QueryRecord> result = await resultIterator.ReadNextAsync(cancellationToken);
                queries.AddRange(result.Select(r => new Query { TBD = r.Query }));
            }

            return queries;
        }

        /// <inheritdoc/>
        public async Task<IList<string>> GetQueryIdsAsync(IList<RegionRef> regions, CancellationToken cancellationToken = default)
        {
            // Validate inputs
            if(regions == null || regions.Count() == 0)
            {
                throw new ArgumentNullException(nameof(regions));
            }

            // Get unique Region IDs from provided collection to build WHERE clause
            IEnumerable<string> regionIds = regions.Select(r => String.Format("'{0}'", r.Id));
            string whereIds = String.Join(",", regionIds);

            // Build query
            string sqlQuery = String.Format("SELECT * FROM c WHERE c.id IN ({0})", whereIds);
            QueryDefinition cosmosQueryDef = new QueryDefinition(sqlQuery);

            // Get results
            FeedIterator<QueryIdRecord> resultIterator = this._queryIdsContainer
                .GetItemQueryIterator<QueryIdRecord>(cosmosQueryDef);
            List<string> queryIds = new List<string>();

            while(resultIterator.HasMoreResults)
            {
                FeedResponse<QueryIdRecord> result = await resultIterator.ReadNextAsync(cancellationToken);
                foreach(QueryIdRecord record in result)
                {
                    queryIds.Add(record.QueryId);
                }
            }

            return queryIds;
        }

        /// <inheritdoc/>
        public async Task PublishAsync(IList<RegionRef> regions, Query query, CancellationToken cancellationToken = default)
        {
            var queryId = Guid.NewGuid().ToString();
            var record = new QueryRecord { Id = queryId, Query = query.TBD };

            ItemResponse<QueryRecord> insertResponse = await this._queryContainer
                 .CreateItemAsync<QueryRecord>(record, new PartitionKey(record.Id), cancellationToken: cancellationToken);

            foreach (var r in regions)
            {
                var queryIdRecord = new QueryIdRecord { Id = r.Id, QueryId = queryId };
                // Create Query in database
                ItemResponse<QueryIdRecord> response = await this._queryIdsContainer
                    .CreateItemAsync<QueryIdRecord>(queryIdRecord, new PartitionKey(queryIdRecord.Id), cancellationToken: cancellationToken);
            }
        }
    }
}
