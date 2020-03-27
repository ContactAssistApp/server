using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TraceDefense.Entities.Geospatial;

namespace TraceDefense.Entities
{
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