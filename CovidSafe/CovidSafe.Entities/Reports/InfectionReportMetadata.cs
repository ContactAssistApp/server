using Newtonsoft.Json;

namespace CovidSafe.Entities.Reports
{
    /// <summary>
    /// Basic <see cref="InfectionReport"/> metadata
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class InfectionReportMetadata
    {
        /// <summary>
        /// <see cref="InfectionReport"/> unique identifier
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        /// <summary>
        /// Timestamp of report creation
        /// </summary>
        /// <remarks>
        /// Reported in milliseconds (ms) since the UNIX epoch.
        /// </remarks>
        [JsonProperty("timestampMs", Required = Required.Always)]
        public long Timestamp { get; set; }
    }
}