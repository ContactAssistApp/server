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
        public const string CONTAINER_NAME = "queries";

        /// <summary>
        /// Creates a new <see cref="CosmosQueryRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">CosmosDB connection factory instance</param>
        public CosmosQueryRepository(CosmosConnectionFactory connectionFactory) : base(connectionFactory)
        {
            // Create container reference
            this._queryContainer = this.Database.GetContainer(CONTAINER_NAME);
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
            string whereIds = String.Join(",", queryIds);

            // Build query
            string sqlQuery = String.Format("SELECT * FROM c WHERE queryId IN ({0})", whereIds);
            QueryDefinition cosmosQueryDef = new QueryDefinition(sqlQuery);

            // Get results
            FeedIterator<Record> resultIterator = this._queryContainer
                .GetItemQueryIterator<Record>(cosmosQueryDef);
            var queries = new List<Query>();

            while (resultIterator.HasMoreResults)
            {
                FeedResponse<Record> result = await resultIterator.ReadNextAsync(cancellationToken);
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
            string sqlQuery = String.Format("SELECT id FROM c WHERE regionId IN ({0})", whereIds);
            QueryDefinition cosmosQueryDef = new QueryDefinition(sqlQuery);

            // Get results
            FeedIterator<Record> resultIterator = this._queryContainer
                .GetItemQueryIterator<Record>(cosmosQueryDef);
            List<string> queryIds = new List<string>();

            while(resultIterator.HasMoreResults)
            {
                FeedResponse<Record> result = await resultIterator.ReadNextAsync(cancellationToken);
                foreach(Record record in result)
                {
                    queryIds.Add(record.Id);
                }
            }

            return queryIds;
        }

        /// <inheritdoc/>
        public async Task PublishAsync(IList<RegionRef> regions, Query query, CancellationToken cancellationToken = default)
        {
            foreach (var r in regions)
            {
                var record = new Record { Id = Guid.NewGuid().ToString(), Query = query.TBD, RegionId = r.Id };
                // Create Query in database
                ItemResponse<Record> insertResponse = await this._queryContainer
                    .CreateItemAsync<Record>(record, new PartitionKey(record.RegionId), cancellationToken: cancellationToken);
            }
        }
    }
}
