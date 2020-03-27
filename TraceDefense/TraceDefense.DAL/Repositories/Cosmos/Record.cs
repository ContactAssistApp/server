using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TraceDefense.DAL.Repositories.Cosmos
{
    public class QueryRecord
    {
        [JsonProperty(PropertyName = "id")]
        [Required]
        public string Id { get; set; }

        public string Query { get; set; }

    }

    public class QueryIdRecord
    {
        [JsonProperty(PropertyName = "id")]
        [Required]
        public string Id { get; set; }

        public string QueryId { get; set; }

    }
}
