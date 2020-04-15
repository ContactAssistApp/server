using System;
using System.ComponentModel.DataAnnotations;

using CovidSafe.DAL.Helpers;
using CovidSafe.Entities.Protos;
using Newtonsoft.Json;

namespace CovidSafe.DAL.Repositories.Cosmos.Records
{
    /// <summary>
    /// <see cref="MatchMessage"/> implementation of <see cref="CosmosRecord{T}"/>
    /// </summary>
    public class MatchMessageRecord : CosmosRecord<MatchMessage>
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
        /// Size of the record <see cref="MatchMessage"/>, in bytes
        /// </summary>
        [JsonProperty("size", Required = Required.Always)]
        [Required]
        public long Size { get; set; }
        /// <summary>
        /// Current version of record schema
        /// </summary>
        [JsonIgnore]
        public const string CURRENT_RECORD_VERSION = "1.0.4";

        /// <summary>
        /// Creates a new <see cref="MatchMessageRecord"/> instance
        /// </summary>
        public MatchMessageRecord()
        {
        }

        /// <summary>
        /// Creates a new <see cref="MatchMessageRecord"/> instance
        /// </summary>
        /// <param name="message"><see cref="MatchMessage"/> to store</param>
        public MatchMessageRecord(MatchMessage message)
        {
            this.Size = PayloadSizeHelper.GetSize(message);
            this.Value = message;
            this.Version = CURRENT_RECORD_VERSION;
        }

        /// <summary>
        /// Generates a new Partition Key value for the record
        /// </summary>
        /// <returns>Partition Key value</returns>
        public static string GetPartitionKey(Region region)
        {
            int lat = (int)PrecisionHelper.Round(region.LatitudePrefix, 0);
            int lon = (int)PrecisionHelper.Round(region.LongitudePrefix, 0);
            
            return $"{lat},{lon}";
        }
    }
}