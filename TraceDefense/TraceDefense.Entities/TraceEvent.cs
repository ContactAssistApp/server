using Newtonsoft.Json;
using TraceDefense.Entities.Geospatial;

namespace TraceDefense.Entities
{
    /// <summary>
    /// Base class for any Trace Event
    /// </summary>
    [JsonObject]
    public abstract class TraceEvent
    {
        /// <summary>
        /// Geographic location of event
        /// </summary>
        [JsonProperty("coordinates", Required = Required.Always)]
        public Coordinate Coordinates { get; set; }
        /// <summary>
        /// Event timestamp (epoch ms)
        /// </summary>
        [JsonProperty("timestamp", Required = Required.Always)]
        public long Timestamp { get; set; }
    }
}
