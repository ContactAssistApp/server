using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TraceDefense.API.Models.Schemas
{
    /// <summary>
    /// Geographic proximity definition
    /// </summary>
    [JsonObject]
    public class GeoProximity
    {
        /// <summary>
        /// Collection of visited <see cref="Location"/> in defined proximity
        /// </summary>
        [JsonProperty("locations")]
        public IList<LocationTime> Locations { get; set; }
        /// <summary>
        /// Minimum tolerance duration required for a match, in seconds
        /// </summary>
        [JsonProperty("durationTolerance")]
        public int DurationTolerance { get; set; }
    }
}
