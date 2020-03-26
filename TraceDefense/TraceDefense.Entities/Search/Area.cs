using Newtonsoft.Json;
using TraceDefense.Entities.Geospatial;

namespace TraceDefense.Entities.Search
{
    /// <summary>
    /// Defined search region
    /// </summary>
    [JsonObject]
    public class Area
    {
        /// <summary>
        /// Geographic point in time
        /// </summary>
        [JsonProperty("point", Required = Required.Always)]
        public Point Point { get; set; }
        /// <summary>
        /// Geographic radius limit (kilometers)
        /// </summary>
        [JsonProperty("radiusKilometers", Required = Required.Always)]
        public int RadiusKilometers { get; set; }
    }
}
