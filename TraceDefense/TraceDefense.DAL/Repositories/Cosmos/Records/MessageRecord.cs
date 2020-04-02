using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.Azure.Cosmos.Spatial;
using Newtonsoft.Json;
using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Repositories.Cosmos.Records
{
    /// <summary>
    /// <see cref="MatchMessage"/> implementation of <see cref="CosmosRecord{T}"/>
    /// </summary>
    public class MatchMessageRecord : CosmosRecord<MatchMessage>
    {
        /// <summary>
        /// Region identifier for each location in this <see cref="ProximityQuery"/>
        /// </summary>
        [JsonProperty("region", Required = Required.Always)]
        [Required]
        public Region Region { get; set; }

        /// <summary>
        /// Creates a new <see cref="ProximityQueryRecord"/> instance
        /// </summary>
        /// <param name="record"><see cref="ProximityQuery"/> data</param>
        public MatchMessageRecord(Region region, MatchMessage message)
        {
            this.Region = region;
            // Set local values
            this.Value = message;
        }
    }
}