using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace TraceDefense.Entities.Interactions
{
    /// <summary>
    /// Match based on Bluetooth unique identifier
    /// </summary>
    [JsonObject]
    public class BluetoothIdMatch : Match
    {
        /// <summary>
        /// Matching Bluetooth identifiers
        /// </summary>
        [JsonProperty("ids", Required = Required.Always)]
        [Required]
        public IList<string> Ids { get; set; }
    }
}