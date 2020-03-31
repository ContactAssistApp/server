using Newtonsoft.Json;

namespace TraceDefense.Entities.Interactions
{
    /// <summary>
    /// Simple <see cref="Query"/> metadata object
    /// </summary>
    public class QueryInfo
    {
        /// <summary>
        /// Unique <see cref="Query"/> identifier
        /// </summary>
        [JsonProperty("query_id", Required = Required.Always)]
        public string QueryId { get; set; }
        /// <summary>
        /// <see cref="Query"/> timestamp, in ms since UNIX epoch
        /// </summary>
        [JsonProperty("query_timestamp", Required = Required.Always)]
        public long Timestamp { get; set; }
    }
}
