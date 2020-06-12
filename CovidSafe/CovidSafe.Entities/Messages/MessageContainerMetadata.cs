using Newtonsoft.Json;

namespace CovidSafe.Entities.Messages
{
    /// <summary>
    /// Basic <see cref="MessageContainer"/> metadata
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MessageContainerMetadata
    {
        /// <summary>
        /// <see cref="MessageContainer"/> unique identifier
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        /// <summary>
        /// Timestamp of <see cref="MessageContainer"/> creation
        /// </summary>
        /// <remarks>
        /// Reported in milliseconds (ms) since the UNIX epoch.
        /// </remarks>
        [JsonProperty("timestampMs", Required = Required.Always)]
        public long Timestamp { get; set; }
    }
}