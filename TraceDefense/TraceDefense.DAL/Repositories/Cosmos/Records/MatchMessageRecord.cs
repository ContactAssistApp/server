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
        /// Unique <see cref="ProximityQuery"/> identifier
        /// </summary>
        [JsonProperty("messageId", Required = Required.Always)]
        [Required]
        public string MessageId { get; set; }

        /// <summary>
        /// Region identifier for each location in this <see cref="MatchMessage"/>
        /// </summary>
        [JsonProperty("locmin", Required = Required.Always)]
        [Required]
        public Point LocMin { get; set; }

        /// <summary>
        /// Region identifier for each location in this <see cref="MatchMessage"/>
        /// </summary>
        [JsonProperty("locmax", Required = Required.Always)]
        [Required]
        public Point LocMax { get; set; }

        /// <summary>
        /// Creates a new <see cref="MatchMessageRecord"/> instance
        /// </summary>
        /// <param name="record"><see cref="ProximityQuery"/> data</param>
        public MatchMessageRecord(MatchMessage message)
        {
            // Set local values
            this.Value = message;
        }
    }
}