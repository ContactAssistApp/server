using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;
using TraceDefense.Entities.Geospatial;

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
        /// <summary>
        /// Unique <see cref="RegionRef"/> identifier
        /// </summary>
        [JsonProperty("regionId")]
        public string RegionId { get; set; }
        /// <summary>
        /// Timestamp of record addition to database, in ms since UNIX epoch
        /// </summary>
        [JsonProperty("timestamp", Required = Required.Always)]
        [Required]
        public long Timestamp { get; set; }
    }
}