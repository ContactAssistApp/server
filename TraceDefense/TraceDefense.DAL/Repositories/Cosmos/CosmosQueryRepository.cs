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
        public async Task<IList<QueryInfo>> GetLatestAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            // Build query
            string sqlQuery = String.Format("SELECT * FROM c WHERE c.value.regionId = @regionId AND c.timestamp > @timestamp");
            QueryDefinition cosmosQueryDef = new QueryDefinition(sqlQuery)
                .WithParameter("@regionId", regionId)
                .WithParameter("@timestamp", lastTimestamp);

            // Get results
            FeedIterator<QueryRecord> resultIterator = this._queryContainer
                .GetItemQueryIterator<QueryRecord>(cosmosQueryDef);
            var queryInfo = new List<QueryInfo>();

            while (resultIterator.HasMoreResults)
            {
                FeedResponse<QueryRecord> result = await resultIterator.ReadNextAsync(cancellationToken);
                QueryRecord obj = result.Resource.FirstOrDefault();
                queryInfo.Add(new QueryInfo
                {
                    QueryId = obj.Id,
                    Timestamp = obj.Value.Timestamp
                });
            }

            return queryInfo;
        }

        /// <inheritdoc/>
        public Task<long> GetLatestRegionSizeAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            // TODO: Implement
            return Task.FromResult((long)100);
        }

        /// <inheritdoc/>
        public async Task<IList<Query>> GetRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
        {
            // Build query
            IEnumerable<string> idsEscaped = ids.Select(i => String.Format("'{0}'", ids));

            string sqlQuery = String.Format("SELECT * FROM c WHERE id IN ({0})", String.Join(",", idsEscaped));
            QueryDefinition cosmosQueryDef = new QueryDefinition(sqlQuery);

            // Get results
            FeedIterator<QueryRecord> resultIterator = this._queryContainer
                .GetItemQueryIterator<QueryRecord>(cosmosQueryDef);
            var queries = new List<Query>();

            while (resultIterator.HasMoreResults)
            {
                FeedResponse<QueryRecord> result = await resultIterator.ReadNextAsync(cancellationToken);
                queries.AddRange(result.Select(r => r.Value));
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
