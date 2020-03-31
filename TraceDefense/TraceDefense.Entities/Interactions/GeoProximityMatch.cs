using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations;

namespace TraceDefense.Entities.Interactions
{
    /// <summary>
    /// Geographic proximity match definition
    /// </summary>
    [JsonObject]
    public class GeoProximityMatch : Match
    {
        /// <summary>
        /// Duration tolerance required for match, in seconds
        /// </summary>
        [JsonProperty("duration_tolerance_secs", Required = Required.Always)]
        [Required]
        public int DurationTolerance { get; set; }
        /// <summary>
        /// Proximity radius to location, in meters
        /// </summary>
        [JsonProperty("proximity_radius_meters", Required = Required.Always)]
        [Required]
        public float ProximityRadius { get; set; }
    }
}
