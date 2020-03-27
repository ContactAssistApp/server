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
        [JsonProperty(PropertyName = "id")]
        [Required]
        public string Id { get; set; }
        /// <summary>
        /// Unique <see cref="RegionRef"/> identifier of <see cref="Query"/>
        /// </summary>
        [JsonProperty(PropertyName = "regionId", Required = Required.Always)]
        [Required]
        public string RegionId { get; set; }
        public string TBD { get; set; }
    }
}