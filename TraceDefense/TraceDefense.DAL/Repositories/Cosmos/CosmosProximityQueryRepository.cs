using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Spatial;
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
            string sqlQuery = "SELECT TOP 1 * FROM c WHERE c.queryId = @queryId";
            QueryDefinition queryDef = new QueryDefinition(sqlQuery)
                .WithParameter("@queryId", queryId);

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
            string sqlQuery = String.Format("SELECT * FROM c WHERE c.regionId = @regionId AND c.timestamp > @timestamp");
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

            string sqlQuery = String.Format("SELECT * FROM c WHERE c.queryId IN ({0})", String.Join(",", idsEscaped));
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
            // Create common properties for each new record
            string queryId = Guid.NewGuid().ToString();
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // Use region identifiers as partition markers
            Dictionary<string, ProximityQueryRecord> newRecords = new Dictionary<string, ProximityQueryRecord>();

            foreach(GeoProximity proximity in query.GeoProximity)
            {
                // Get region identifiers
                foreach(LocationTime loc in proximity.Locations)
                {
                    string regionId = RegionIdProvider.FromLocationTime(loc);

                    if(newRecords.ContainsKey(regionId))
                    {
                        // Add location as reference
                        newRecords[regionId].Regions.Add(
                            new Point(
                                loc.Location.Longitude,
                                loc.Location.Longitude
                            )
                        );
                    }
                    else
                    {
                        // Create new region reference
                        ProximityQueryRecord record = new ProximityQueryRecord(query)
                        {
                            QueryId = queryId,
                            Regions = new List<Point>(),
                            RegionId = regionId,
                            Timestamp = timestamp,
                            Version = query.MessageVersion
                        };
                        record.Regions.Add(
                            new Point(
                                loc.Location.Longitude,
                                loc.Location.Lattitude
                            )
                        );
                        newRecords[regionId] = record;
                    }
                }
            }

            // Start transaction
            foreach(ProximityQueryRecord rec in newRecords.Values)
            {
                ItemResponse<ProximityQueryRecord> response = await this._queryContainer
                    .CreateItemAsync<ProximityQueryRecord>(
                        rec,
                        new PartitionKey(rec.RegionId),
                        cancellationToken: cancellationToken
                    );
            }

            // Return new QueryId
            return queryId;
        }
    }
}
