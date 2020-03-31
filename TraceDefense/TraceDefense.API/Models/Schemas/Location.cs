using Newtonsoft.Json;

namespace TraceDefense.API.Models.Schemas
{
    /// <summary>
    /// Geographic location
    /// </summary>
    [JsonObject]
    public class Location
    {
        /// <summary>
        /// Latitude of coordinate
        /// </summary>
        [JsonProperty("lat", Required = Required.Always)]
        public double Latitude { get; set; }
        /// <summary>
        /// Longitude of coordinate
        /// </summary>
        [JsonProperty("lng", Required = Required.Always)]
        public double Longitude { get; set; }
    }
}