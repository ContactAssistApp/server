using Newtonsoft.Json;

namespace TraceDefense.Entities.Registration
{
    /// <summary>
    /// Device Registration entity
    /// </summary>
    [JsonObject]
    public class DeviceRegistration
    {
        /// <summary>
        /// Unique device identifier
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
        /// <summary>
        /// Device region
        /// </summary>
        [JsonProperty("region", Required = Required.AllowNull)]
        public string Region { get; set; }
    }
}
