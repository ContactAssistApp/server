using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using TraceDefense.DAL.Providers;
using TraceDefense.DAL.Repositories.Cosmos.Records;
using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Repositories.Cosmos
{
    /// <summary>
    /// CosmosDB implementation of <see cref="IProximityQueryRepository"/>
    /// </summary>
    public class CosmosProximityQueryRepository : CosmosRepository, IProximityQueryRepository
    {
        /// <summary>
        /// <see cref="ProximityQuery"/> container object
        /// </summary>
        private Container _queryContainer;

        /// <summary>
        /// Creates a new <see cref="CosmosProximityQueryRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Database connection factory instance</param>
        /// <param name="schemaOptions">Schema options object</param>
        public CosmosProximityQueryRepository(CosmosConnectionFactory connectionFactory, IOptionsMonitor<CosmosCovidSafeSchemaOptions> schemaOptions) : base(connectionFactory, schemaOptions)
        {
            // Create container reference
            this._queryContainer = this.Database
                .GetContainer(this.SchemaOptions.QueryContainerName);
        }

        /// <inheritdoc/>
        public async Task<ProximityQuery> GetAsync(string queryId, CancellationToken cancellationToken = default)
        {
            // Build query
            string sqlQuery = "SELECT * FROM c WHERE id = @id";
            QueryDefinition queryDef = new QueryDefinition(sqlQuery)
                .WithParameter("@id", queryId);

            // Get results
            FeedIterator<ProximityQueryRecord> iterator = this._queryContainer
                .GetItemQueryIterator<ProximityQueryRecord>(queryDef);
            ProximityQueryRecord instance = null;

            while(iterator.HasMoreResults)
            {
                FeedResponse<ProximityQueryRecord> result = await iterator.ReadNextAsync(cancellationToken);
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
        public async Task<IEnumerable<QueryInfo>> GetLatestAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            // Build query
            string sqlQuery = String.Format("SELECT * FROM c WHERE c.value.regionId = @regionId AND c.timestamp > @timestamp");
            QueryDefinition cosmosQueryDef = new QueryDefinition(sqlQuery)
                .WithParameter("@regionId", regionId)
                .WithParameter("@timestamp", lastTimestamp);

            // Get results
            FeedIterator<ProximityQueryRecord> resultIterator = this._queryContainer
                .GetItemQueryIterator<ProximityQueryRecord>(cosmosQueryDef);
            var queryInfo = new List<QueryInfo>();

            while (resultIterator.HasMoreResults)
            {
                FeedResponse<ProximityQueryRecord> result = await resultIterator.ReadNextAsync(cancellationToken);
                ProximityQueryRecord obj = result.Resource.FirstOrDefault();
                queryInfo.Add(new QueryInfo
                {
                    QueryId = obj.Id,
                    QueryTimestamp = UtcTimeHelper.ToUtcTime(obj.Timestamp)
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
        public async Task<IEnumerable<ProximityQuery>> GetRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
        {
            // Build query
            IEnumerable<string> idsEscaped = ids.Select(i => String.Format("'{0}'", ids));

            string sqlQuery = String.Format("SELECT * FROM c WHERE id IN ({0})", String.Join(",", idsEscaped));
            QueryDefinition cosmosQueryDef = new QueryDefinition(sqlQuery);

            // Get results
            FeedIterator<ProximityQueryRecord> resultIterator = this._queryContainer
                .GetItemQueryIterator<ProximityQueryRecord>(cosmosQueryDef);
            var queries = new List<ProximityQuery>();

            while (resultIterator.HasMoreResults)
            {
                FeedResponse<ProximityQueryRecord> result = await resultIterator.ReadNextAsync(cancellationToken);
                queries.AddRange(result.Select(r => r.Value));
            }

            return queries;
        }

        /// <inheritdoc/>
        public async Task<string> InsertAsync(ProximityQuery query, CancellationToken cancellationToken = default)
        {
            // Create new QueryRecord
            ProximityQueryRecord toStore = new ProximityQueryRecord(query, Guid.NewGuid().ToString());
            toStore.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // Determine partition
            string regionId = RegionIdProvider.FromGeoProximity(query.GeoProximity.ToList());

            // Create Query in database
            ItemResponse<ProximityQueryRecord> response = await this._queryContainer
                .CreateItemAsync<ProximityQueryRecord>(toStore, new PartitionKey(toStore.RegionId), cancellationToken: cancellationToken);

            // Return unique identifier of new resource
            return response.Resource.Id;
        }
    }
}
