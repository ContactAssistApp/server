using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;
using TraceDefense.Entities;
using TraceDefense.Entities.Geospatial;

namespace TraceDefense.DAL.Repositories.CosmosDb.Records
{
    /// <summary>
    /// <see cref="Query"/> object wrapper for use in Table Storage
    /// </summary>
    public class QueryRecord : TableEntity
    {
        /// <summary>
        /// Query record contents
        /// </summary>
        public Query QueryContents { get; set; }

        /// <summary>
        /// Creates a new <see cref="QueryRecord"/> entity
        /// </summary>
        public QueryRecord()
        {
            // Empty constructor required by contract
        }

        /// <summary>
        /// Creates a new <see cref="QueryRecord"/> entity by RegionRef and ID
        /// </summary>
        /// <param name="region">Record region (Partition Key)</param>
        /// <param name="queryId">Query ID (Row Key)</param>
        public QueryRecord(RegionRef region, int queryId)
        {
            this.PartitionKey = region.Id.ToString();
            this.RowKey = queryId.ToString();
        }

        /// <summary>
        /// Creates a new <see cref="QueryRecord"/> entity by <see cref="RegionRef"/>, ID, and contents
        /// </summary>
        /// <param name="region">Record region (Partition Key)</param>
        /// <param name="queryId">Query ID (Row Key)</param>
        /// <param name="queryContent"><see cref="Query"/> contents</param>
        public QueryRecord(RegionRef region, int queryId, Query queryContent)
        {
            this.PartitionKey = region.Id.ToString();
            this.RowKey = queryId.ToString();
            this.QueryContents = queryContent;
        }
    }
}
