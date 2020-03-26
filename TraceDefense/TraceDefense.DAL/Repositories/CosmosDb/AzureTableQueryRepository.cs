using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos.Table;
using TraceDefense.DAL.Providers;
using TraceDefense.DAL.Repositories.CosmosDb.Records;
using TraceDefense.Entities;
using TraceDefense.Entities.Geospatial;

namespace TraceDefense.DAL.Repositories.CosmosDb
{
    /// <summary>
    /// Cosmos DB Table API 
    /// </summary>
    public class AzureTableQueryRepository : AzureStorageRepository, IQueryRepository
    {
        /// <summary>
        /// Cloud Table client object
        /// </summary>
        private CloudTableClient _tableClient;
        /// <summary>
        /// Name of table used to store <see cref="QueryRecord"/> objects
        /// </summary>
        private const string TABLE_NAME = "queries";

        /// <summary>
        /// Creates a new <see cref="AzureTableQueryRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory"><see cref="AzureStorageConnectionFactory"/> instance</param>
        public AzureTableQueryRepository(AzureStorageConnectionFactory connectionFactory) : base(connectionFactory)
        {
            // Create table client
            this._tableClient = this.ConnectionFactory
                .StorageAccount
                .CreateCloudTableClient(new TableClientConfiguration());
        }
        
        /// <inheritdoc/>
        public Task<IList<Query>> GetQueries(IList<int> queryIds)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IList<Query>> GetQueryIds(IList<RegionRef> regions)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<Query> Publish(IList<RegionRef> regions, Query query)
        {
            throw new NotImplementedException();
        }
    }
}
