using Newtonsoft.Json;

namespace TraceDefense.API.Models.Schemas
{
    /// <summary>
    /// <see cref="Location"/> with timestamp
    /// </summary>
    public class LocationTime
    {
        /// <summary>
        /// Geographic <see cref="Location"/>
        /// </summary>
        [JsonProperty("location", Required = Required.Always)]
        public Location Location { get; set; }
        /// <summary>
        /// Timestamp of <see cref="Location"/> visit, in ms since UNIX epoch
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
