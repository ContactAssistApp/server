using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
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
        /// Creates a new <see cref="CosmosQueryRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Database connection factory instance</param>
        /// <param name="schemaOptions">Schema options object</param>
        public CosmosQueryRepository(CosmosConnectionFactory connectionFactory, IOptionsMonitor<CosmosCovidSafeSchemaOptions> schemaOptions) : base(connectionFactory, schemaOptions)
        {
            // Create container reference
            this._queryContainer = this.Database
                .GetContainer(this.SchemaOptions.QueryContainerName);
        }

        /// <inheritdoc/>
        public async Task<IList<Query>> GetQueriesAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            // Build query
            string sqlQuery = String.Format("SELECT * FROM c WHERE c.RegionId = @regionId AND c.Timestamp > @timestamp");
            QueryDefinition cosmosQueryDef = new QueryDefinition(sqlQuery)
                .WithParameter("@regionId", regionId)
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

            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

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
