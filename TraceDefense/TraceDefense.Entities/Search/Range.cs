using Newtonsoft.Json;

namespace TraceDefense.Entities.Search
{
    /// <summary>
    /// Search range parameters
    /// </summary>
    [JsonObject]
    public class Range
    {
        /// <summary>
        /// Geographic area to include in search (kilometers)
        /// </summary>
        [JsonProperty("radiusKilometers", Required = Required.Always)]
        public int RadiusKilometers { get; set; }
        /// <summary>
        /// Amount of time (in minutes) to include from current timestamp
        /// </summary>
        [JsonProperty("timeMins", Required = Required.Always)]
        public int TimeMinutes { get; set; }
    }
}
