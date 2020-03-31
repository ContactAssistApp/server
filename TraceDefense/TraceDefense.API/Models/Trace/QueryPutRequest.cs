﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

using TraceDefense.Entities.Geospatial;
using TraceDefense.Entities.Schemas;

namespace TraceDefense.API.Models.Trace
{
    /// <summary>
    /// Request model for HTTP PUT operations against /Trace/Query
    /// </summary>
    [JsonObject]
    public class QueryPutRequest
    {
        /// <summary>
        /// Geographic area tagged by <see cref="Query"/>
        /// </summary>
        [JsonProperty("area", Required = Required.Always)]
        [Required]
        public Area Area { get; set; }
        /// <summary>
        /// <see cref="Query"/> content
        /// </summary>
        [JsonProperty("query", Required = Required.Always)]
        [Required]
        public Query Query { get; set; }
    }
}