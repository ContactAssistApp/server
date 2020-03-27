using Newtonsoft.Json;

namespace TraceDefense.Entities.Geospatial
{
    /// <summary>
    /// Defined search region
    /// </summary>
    [JsonObject]
    public class Area
    {
        /// <summary>
        /// First corner of search region
        /// </summary>
        [JsonProperty("first", Required = Required.Always)]
        public Location First { get; set; }


        /// <summary>
        /// Second corner of search region
        /// </summary>
        [JsonProperty("second", Required = Required.Always)]
        public Location Second { get; set; }
    }
}
