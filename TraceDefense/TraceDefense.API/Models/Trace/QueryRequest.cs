using Newtonsoft.Json;
using TraceDefense.Entities.Search;

namespace TraceDefense.API.Models.Trace
{
    /// <summary>
    /// Query request
    /// </summary>
    [JsonObject]
    public class QueryRequest
    {
        /// <summary>
        /// Geographic <see cref="Area"/> used to scope query
        /// </summary>
        [JsonProperty("area", Required = Required.Always)]
        public Area Area { get; set; }
        /// <summary>
        /// Query text
        /// </summary>
        [JsonProperty("query", Required = Required.Always)]
        public string Query { get; set; }
        /// <summary>
        /// <see cref="TimeRange"/> used to scope query
        /// </summary>
        [JsonProperty("timeRange", Required = Required.Always)]
        public TimeRange TimeRange { get; set; }
    }
}
