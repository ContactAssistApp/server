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

        public string RegionId { get; set; }

        public int Timestamp { get; set; }

    }
}
