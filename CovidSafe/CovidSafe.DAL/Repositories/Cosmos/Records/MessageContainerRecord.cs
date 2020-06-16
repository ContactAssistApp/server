using System.ComponentModel.DataAnnotations;

using CovidSafe.DAL.Helpers;
using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Messages;
using Newtonsoft.Json;

namespace CovidSafe.DAL.Repositories.Cosmos.Records
{
    /// <summary>
    /// <see cref="MessageContainer"/> implementation of <see cref="CosmosRecord{T}"/>
    /// </summary>
    public class MessageContainerRecord : CosmosRecord<MessageContainer>
    {
        /// <summary>
        /// Boundary allowed by <see cref="MessageContainer"/> region
        /// </summary>
        [JsonProperty("RegionBoundary", Required = Required.Always)]
        [Required]
        public RegionBoundary RegionBoundary { get; set; }

        /// <summary>
        /// <see cref="MessageContainer"/> region
        /// </summary>
        [JsonProperty("Region", Required = Required.Always)]
        [Required]
        public Region Region { get; set; }

        /// <summary>
        /// Size of the record <see cref="MessageContainer"/>, in bytes
        /// </summary>
        [JsonProperty("size", Required = Required.Always)]
        [Required]
        public long Size { get; set; }
        
        /// <summary>
        /// Current version of record schema
        /// </summary>
        [JsonIgnore]
        public const string CURRENT_RECORD_VERSION = "4.0.0";

        /// <summary>
        /// Creates a new <see cref="MessageContainerRecord"/> instance
        /// </summary>
        public MessageContainerRecord()
        {
        }

        /// <summary>
        /// Creates a new <see cref="MessageContainerRecord"/> instance
        /// </summary>
        /// <param name="report"><see cref="MessageContainer"/> to store</param>
        public MessageContainerRecord(MessageContainer report)
        {
            this.Size = PayloadSizeHelper.GetSize(report);
            this.Value = report;
            this.Version = CURRENT_RECORD_VERSION;
        }

        /// <summary>
        /// Generates a new Partition Key value for the record
        /// </summary>
        /// <returns>Partition Key value</returns>
        public static string GetPartitionKey(Region region)
        {
            return $"{region.LatitudePrefix},{region.LongitudePrefix},{region.Precision}";
        }
    }
}