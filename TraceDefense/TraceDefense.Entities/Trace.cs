using Newtonsoft.Json;
using TraceDefense.Entities.Geospatial;

namespace TraceDefense.Entities
{
    /// <summary>
    /// Base class for trace entities
    /// </summary>
    [JsonObject]
    public class Trace
    {
        /// <summary>
        /// Geographic location of event
        /// </summary>
        [JsonProperty("coordinates", Required = Required.Always)]
        public Coordinate Coordinates { get; set; }
        /// <summary>
        /// Device identifier which generated this event
        /// </summary>
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
        /// <summary>
        /// Event timestamp (epoch ms)
        /// </summary>
        [JsonProperty("timestamp", Required = Required.Always)]
        public long Timestamp { get; set; }
    }
}
