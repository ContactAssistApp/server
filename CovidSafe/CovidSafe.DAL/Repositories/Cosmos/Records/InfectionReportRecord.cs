using System.ComponentModel.DataAnnotations;

using CovidSafe.DAL.Helpers;
using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Reports;
using Newtonsoft.Json;

namespace CovidSafe.DAL.Repositories.Cosmos.Records
{
    /// <summary>
    /// <see cref="InfectionReport"/> implementation of <see cref="CosmosRecord{T}"/>
    /// </summary>
    public class InfectionReportRecord : CosmosRecord<InfectionReport>
    {
        /// <summary>
        /// Boundary allowed by <see cref="InfectionReport"/> region
        /// </summary>
        [JsonProperty("RegionBoundary", Required = Required.Always)]
        [Required]
        public RegionBoundary RegionBoundary { get; set; }
        /// <summary>
        /// Size of the record <see cref="InfectionReport"/>, in bytes
        /// </summary>
        [JsonProperty("size", Required = Required.Always)]
        [Required]
        public long Size { get; set; }
        /// <summary>
        /// Current version of record schema
        /// </summary>
        [JsonIgnore]
        public const string CURRENT_RECORD_VERSION = "2.0.0";

        /// <summary>
        /// Creates a new <see cref="InfectionReportRecord"/> instance
        /// </summary>
        public InfectionReportRecord()
        {
        }

        /// <summary>
        /// Creates a new <see cref="InfectionReportRecord"/> instance
        /// </summary>
        /// <param name="report"><see cref="InfectionReport"/> to store</param>
        public InfectionReportRecord(InfectionReport report)
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
            int lat = (int)PrecisionHelper.Round(region.LatitudePrefix, 0);
            int lon = (int)PrecisionHelper.Round(region.LongitudePrefix, 0);
            
            return $"{lat},{lon}";
        }
    }
}