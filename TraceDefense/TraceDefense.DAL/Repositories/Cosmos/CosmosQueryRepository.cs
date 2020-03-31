using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using TraceDefense.DAL.Repositories.Cosmos.Records;
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
        public async Task<Query> GetAsync(string queryId, CancellationToken cancellationToken = default)
        {
            // Build query
            string sqlQuery = "SELECT * FROM c WHERE id = @id";
            QueryDefinition queryDef = new QueryDefinition(sqlQuery)
                .WithParameter("@id", queryId);

            // Get results
            FeedIterator<QueryRecord> iterator = this._queryContainer
                .GetItemQueryIterator<QueryRecord>(queryDef);
            QueryRecord instance = null;

            while(iterator.HasMoreResults)
            {
                FeedResponse<QueryRecord> result = await iterator.ReadNextAsync(cancellationToken);
                instance = result.Resource.FirstOrDefault();
            }

            if(instance != null)
            {
                return instance.Value;
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<IList<Query>> GetByRegionAsync(string regionId, int precision, long lastTimestamp, CancellationToken cancellationToken = default)
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
        public async Task<string> InsertAsync(Query query, CancellationToken cancellationToken = default)
        {
            query.Id = Guid.NewGuid().ToString();
            query.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // Create new QueryRecord
            QueryRecord toStore = new QueryRecord(query);

            // Update region

            // Create Query in database
            ItemResponse<Query> response = await this._queryContainer
                .CreateItemAsync<Query>(query, new PartitionKey(query.RegionId), cancellationToken: cancellationToken);

            // Return unique identifier of new resource
            return response.Resource.Id;
        }
    }
}
