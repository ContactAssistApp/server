using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;
using TraceDefense.Entities.Geospatial;

namespace TraceDefense.Entities.Interactions
{
    /// <summary>
    /// <see cref="Location"/> at given point in time
    /// </summary>
    [JsonObject]
    public class LocationTime
    {
        /// <summary>
        /// Referenced <see cref="Location"/>
        /// </summary>
        [JsonProperty("location", Required = Required.Always)]
        [Required]
        public Location Location { get; set; }
        /// <summary>
        /// Time at location, in ms since UNIX epoch
        /// </summary>
        [JsonProperty("time", Required = Required.Always)]
        [Required]
        public long Time { get; set; }
    }
}
