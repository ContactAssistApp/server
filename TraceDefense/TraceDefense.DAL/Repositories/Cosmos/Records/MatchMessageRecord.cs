using System.ComponentModel.DataAnnotations;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Newtonsoft.Json;
using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Repositories.Cosmos.Records
{
    /// <summary>
    /// <see cref="MatchMessage"/> implementation of <see cref="CosmosRecord{T}"/>
    /// </summary>
    public class MatchMessageRecord : CosmosRecord<string>
    {
        /// <summary>
        /// Boundary allowed by <see cref="MatchMessage"/> region
        /// </summary>
        [JsonProperty("RegionBoundary", Required = Required.Always)]
        [Required]
        public RegionBoundaryProperty RegionBoundary { get; set; }
        /// <summary>
        /// <see cref="Region"/> applicable to the <see cref="MatchMessage"/>
        /// </summary>
        [JsonProperty("Region", Required = Required.Always)]
        [Required]
        public RegionProperty Region { get; set; }
        /// <summary>
        /// Unique <see cref="Region"/> identifier
        /// </summary>
        /// <remarks>
        /// Used as partition key
        /// </remarks>
        [JsonProperty("regionId", Required = Required.Always)]
        [Required]
        public string RegionId { get; set; }
        /// <summary>
        /// Size of the record <see cref="MatchMessage"/>, in bytes
        /// </summary>
        [JsonProperty("size", Required = Required.Always)]
        [Required]
        public long Size { get; set; }

        /// <summary>
        /// Creates a new <see cref="MatchMessageRecord"/> instance
        /// </summary>
        public MatchMessageRecord()
        {
        }
    }
}