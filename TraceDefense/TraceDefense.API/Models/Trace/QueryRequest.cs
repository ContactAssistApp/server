using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TraceDefense.Entities.Search;

namespace TraceDefense.API.Models.Trace
{
    /// <summary>
    /// Query request
    /// </summary>
    [JsonObject]
    public class QueryRequest
    {
        /// <summary>
        /// Geographic <see cref="Area"/> used to scope query
        /// </summary>
        [JsonProperty("area", Required = Required.Always)]
        [Required]
        public Area Area { get; set; }
        /// <summary>
        /// Query text
        /// </summary>
        [JsonProperty("query", Required = Required.Always)]
        [Required]
        public string Query { get; set; }
    }
}
