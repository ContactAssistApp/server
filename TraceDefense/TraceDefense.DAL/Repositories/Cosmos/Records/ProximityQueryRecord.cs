using System.ComponentModel.DataAnnotations;

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
        /// Region identifier
        /// </summary>
        /// <remarks>
        /// Used as partition field
        /// </remarks>
        [JsonProperty("regionId", Required = Required.Always)]
        [Required]
        public string RegionId { get; set; }

        /// <summary>
        /// Creates a new <see cref="ProximityQueryRecord"/> instance
        /// </summary>
        /// <param name="record"><see cref="ProximityQuery"/> data</param>
        public ProximityQueryRecord(ProximityQuery record)
        {
            // Set local values
            this.Value = record;
        }

        /// <summary>
        /// Creates a new <see cref="ProximityQueryRecord"/> instance
        /// </summary>
        /// <param name="record"><see cref="ProximityQuery"/> data</param>
        /// <param name="id">Unique <see cref="ProximityQuery"/> identifier</param>
        public ProximityQueryRecord(ProximityQuery record, string id)
        {
            // Set local values
            this.Id = id;
            this.Value = record;
        }

        /// <summary>
        /// Creates a new <see cref="ProximityQueryRecord"/> instance
        /// </summary>
        /// <param name="record"><see cref="ProximityQuery"/> data</param>
        /// <param name="id">Unique <see cref="ProximityQuery"/> identifier</param>
        /// <param name="regionId">Region identifier</param>
        public ProximityQueryRecord(ProximityQuery record, string id, string regionId)
        {
            // Set local values
            this.Id = id;
            this.RegionId = regionId;
            this.Value = record;
        }
    }
}
