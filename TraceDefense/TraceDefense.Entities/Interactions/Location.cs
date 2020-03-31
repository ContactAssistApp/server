using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace TraceDefense.Entities.Interactions
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
        [JsonProperty("lattitude", Required = Required.Always)]
        [Required]
        public double Latitude { get; set; }
        /// <summary>
        /// Longitude
        /// </summary>
        [JsonProperty("longitude", Required = Required.Always)]
        [Required]
        public double Longitude { get; set; }
        /// <summary>
        /// Radius around location, in meters
        /// </summary>
        [JsonProperty("radius_meters", Required = Required.Always)]
        [Required]
        public int Radius { get; set; }
    }
}
