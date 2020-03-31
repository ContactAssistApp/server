using Microsoft.Azure.Cosmos.Spatial;
using Newtonsoft.Json;
using TraceDefense.Entities.Interactions;

namespace TraceDefense.DAL.Repositories.Cosmos.Records
{
    /// <summary>
    /// <see cref="Query"/> implementation of <see cref="CosmosRecord{T}"/>
    /// </summary>
    public class QueryRecord : CosmosRecord<Query>
    {
        /// <summary>
        /// Region covered by <see cref="Query"/>
        /// </summary>
        [JsonProperty("region", Required = Required.Always)]
        public Point Region { get; set; }

        /// <summary>
        /// Creates a new <see cref="QueryRecord"/> instance
        /// </summary>
        /// <param name="record"><see cref="Query"/> data</param>
        public QueryRecord(Query record)
        {
            // Set local values
            this.Id = record.Id;
            this.Value = record;
        }

        /// <summary>
        /// Creates a new <see cref="QueryRecord"/> instance
        /// </summary>
        /// <param name="record"><see cref="Query"/> data</param>
        /// <param name="id">Unique <see cref="Query"/> identifier</param>
        public QueryRecord(Query record, string id)
        {
            // Set local values
            this.Id = id;
            this.Value = record;
        }
    }
}
