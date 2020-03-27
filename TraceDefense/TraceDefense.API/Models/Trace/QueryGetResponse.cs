using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;
using TraceDefense.Entities;

namespace TraceDefense.API.Models.Trace
{
    /// <summary>
    /// Response model for HTTP GET operations against /Trace/Query
    /// </summary>
    [JsonObject]
    public class QueryGetResponse
    {
        /// <summary>
        /// Collection of <see cref="Query"/> objects matching request
        /// </summary>
        [JsonProperty("queries", Required = Required.Always)]
        [Required]
        public IList<Query> Queries { get; set; }

        /// <summary>
        /// Timestamp of server response, in miliseconds, from the Unix epoch
        /// </summary>
        [JsonProperty("timestamp", Required = Required.Always)]
        [Required]
        public long Timestamp { get; set; }
    }
}
