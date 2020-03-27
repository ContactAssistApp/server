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
        /// <summary>
        /// Name of <see cref="Query"/> container
        /// </summary>
        public const string QUERIES_CONTAINER_NAME = "queries_v3";

        /// <summary>
        /// Creates a new <see cref="CosmosQueryRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">CosmosDB connection factory instance</param>
        public CosmosQueryRepository(CosmosConnectionFactory connectionFactory) : base(connectionFactory)
        {
            // Create container reference
            this._queryContainer = this.Database.GetContainer(QUERIES_CONTAINER_NAME);
        }

        /// <inheritdoc/>
        public async Task<IList<Query>> GetQueriesAsync(RegionRef region, int lastTimestamp, CancellationToken cancellationToken = default)
        {
            // Build query
            string sqlQuery = String.Format("SELECT * FROM c WHERE c.RegionId = @regionId AND c.Timestamp > @timestamp");
            QueryDefinition cosmosQueryDef = new QueryDefinition(sqlQuery)
                .WithParameter("@regionId", region.Id)
                .WithParameter("@timestamp", lastTimestamp);

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
        public async Task PublishAsync(IList<RegionRef> regions, Query query, CancellationToken cancellationToken = default)
        {
            var queryId = Guid.NewGuid().ToString();

            var timestamp = TimestampProvider.GetTimestamp();

            foreach (var r in regions)
            {
                var record = new QueryRecord { Id = queryId, Query = query.TBD, RegionId = r.Id, Timestamp = timestamp };
                // Create Query in database
                ItemResponse<QueryRecord> response = await this._queryContainer
                    .CreateItemAsync<QueryRecord>(record, new PartitionKey(record.RegionId), cancellationToken: cancellationToken);
            }
        }
    }
}
