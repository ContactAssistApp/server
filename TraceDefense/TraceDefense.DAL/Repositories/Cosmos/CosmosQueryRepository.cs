using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using TraceDefense.Entities.Geospatial;
using TraceDefense.Entities.Interactions;

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
            string sqlQuery = String.Format("SELECT * FROM c WHERE c.regionId = @regionId AND c.timestamp > @timestamp");
            QueryDefinition cosmosQueryDef = new QueryDefinition(sqlQuery)
                .WithParameter("@regionId", regionId)
                .WithParameter("@timestamp", lastTimestamp);

            // Get results
            FeedIterator<Query> resultIterator = this._queryContainer
                .GetItemQueryIterator<Query>(cosmosQueryDef);
            var queries = new List<Query>();

            while (resultIterator.HasMoreResults)
            {
                FeedResponse<Query> result = await resultIterator.ReadNextAsync(cancellationToken);
                queries.AddRange(result);
            }

            return queries;
        }

        /// <inheritdoc/>
        public async Task PublishAsync(IList<RegionRef> regions, Query query, CancellationToken cancellationToken = default)
        {
            Query baseRecord = query;
            query.Id = Guid.NewGuid().ToString();
            query.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            foreach (var r in regions)
            {
                // Tag query with appropriate RegionID
                Query newRecord = baseRecord;
                query.RegionId = r.Id;
                // Create Query in database
                ItemResponse<Query> response = await this._queryContainer
                    .CreateItemAsync<Query>(newRecord, new PartitionKey(newRecord.RegionId), cancellationToken: cancellationToken);
            }
        }
    }
}
