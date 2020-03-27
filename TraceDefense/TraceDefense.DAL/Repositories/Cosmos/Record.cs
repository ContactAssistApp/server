using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;
using TraceDefense.Entities.Geospatial;

namespace TraceDefense.DAL.Repositories.Cosmos
{
    /// <summary>
    /// <see cref="Query"/> database record
    /// </summary>
    public class QueryRecord
    {
        /// <summary>
        /// Unique record identifier
        /// </summary>
        [JsonProperty("id")]
        [Required]
        public string Id { get; set; }
        /// <summary>
        /// <see cref="Query"/> contents
        /// </summary>
        [JsonProperty("query")]
        [Required]
        public string Query { get; set; }
        /// <summary>
        /// Unique <see cref="RegionRef"/> identifier tied to this <see cref="Query"/>
        /// </summary>
        [JsonProperty("regionId")]
        public string RegionId { get; set; }
        /// <summary>
        /// Timestamp, in ms since Unix epoch, of when record was added to the database
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

    }
}
