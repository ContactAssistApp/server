using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.Azure.Cosmos.Spatial;
using Newtonsoft.Json;
using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Repositories.Cosmos.Records
{
    /// <summary>
    /// <see cref="ProximityQuery"/> implementation of <see cref="CosmosRecord{T}"/>
    /// </summary>
    public class ProximityQueryRecord : CosmosRecord<ProximityQuery>
    {
        /// <summary>
        /// Unique <see cref="ProximityQuery"/> identifier
        /// </summary>
        [JsonProperty("queryId", Required = Required.Always)]
        [Required]
        public string QueryId { get; set; }
        /// <summary>
        /// Region identifier for each location in this <see cref="ProximityQuery"/>
        /// </summary>
        [JsonProperty("regionId", Required = Required.Always)]
        [Required]
        public string RegionId { get; set; }
        /// <summary>
        /// Collection of <see cref="Point"/> objects corresponding to regions 
        /// covered by this <see cref="ProximityQueryRecord"/>
        /// </summary>
        [JsonProperty("Regions", Required = Required.Always)]
        [Required]
        public IList<Point> Regions { get; set; }

        /// <summary>
        /// Creates a new <see cref="ProximityQueryRecord"/> instance
        /// </summary>
        /// <param name="record"><see cref="ProximityQuery"/> data</param>
        public ProximityQueryRecord(ProximityQuery record)
        {
            // Set local values
            this.Value = record;
        }
    }
}