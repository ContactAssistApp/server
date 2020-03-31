using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace TraceDefense.Entities.Interactions
{
    /// <summary>
    /// Base unit of trace data
    /// </summary>
    public class Query
    {
        /// <summary>
        /// Unique <see cref="Query"/> identifier used by CosmosDB
        /// </summary>
        [JsonProperty(PropertyName = "tbd")]
        [Required]
        public string TBD { get; set; }
    }
}