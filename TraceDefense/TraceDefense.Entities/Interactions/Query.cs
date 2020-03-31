using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace TraceDefense.Entities.Interactions
{
    /// <summary>
    /// Proximity query object definition
    /// </summary>
    [JsonObject]
    public class Query
    {
        /// <summary>
        /// Unique <see cref="Query"/> identifier
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }
        /// <summary>
        /// Collection of <see cref="BluetoothIdMatch"/> objects
        /// </summary>
        [JsonProperty("id_list")]
        public IList<BluetoothIdMatch> BluetoothIds { get; set; }
        /// <summary>
        /// Collection of <see cref="GeoProximityMatch"/> objects
        /// </summary>
        [JsonProperty("geo_proximity")]
        public IList<GeoProximityMatch> GeoProximities { get; set; }
    }
}