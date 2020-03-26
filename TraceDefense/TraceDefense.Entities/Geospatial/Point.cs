using Newtonsoft.Json;

namespace TraceDefense.Entities.Geospatial
{
    /// <summary>
    /// Geographic location at a point in time
    /// </summary>
    [JsonObject]
    public class Point
    {
        /// <summary>
        /// Geographic coordinates of point
        /// </summary>
        [JsonProperty("coordinates", Required = Required.Always)]
        public Coordinate Coordinates { get; set; }
        /// <summary>
        /// Point in time (epoch ms)
        /// </summary>
        [JsonProperty("timestampMs", Required = Required.Always)]
        public long Timestamp { get; set; }
    }
}
