using Newtonsoft.Json;

namespace TraceDefense.Entities.Search
{
    /// <summary>
    /// Time Range used to scope a search
    /// </summary>
    [JsonObject]
    public class TimeRange
    {
        /// <summary>
        /// End of time range (epoch ms)
        /// </summary>
        [JsonProperty("end", Required = Required.Always)]
        public long EndTimeS { get; set; }
        /// <summary>
        /// Start of time range (epoch ms)
        /// </summary>
        [JsonProperty("start", Required = Required.Always)]
        public long StartTimeS { get; set; }
    }
}
