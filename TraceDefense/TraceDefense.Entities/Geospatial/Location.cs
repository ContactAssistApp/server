using Newtonsoft.Json;

namespace TraceDefense.Entities.Geospatial
{
    /// <summary>
    /// Geographical location
    /// </summary>
    [JsonObject]
    public class Location
    {
        /// <summary>
        /// Latitude
        /// </summary>
        [JsonProperty("lat", Required = Required.Always)]
        public double Latitude { get; set; }
        /// <summary>
        /// Longitude
        /// </summary>
        [JsonProperty("lng", Required = Required.Always)]
        public double Longitude { get; set; }
    }
}
