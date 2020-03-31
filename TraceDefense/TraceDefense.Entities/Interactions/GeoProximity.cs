using Newtonsoft.Json;

namespace TraceDefense.Entities.Interactions
{
    /// <summary>
    /// Geographic proximity match definition
    /// </summary>
    [JsonObject]
    public class GeoProximity
    {
        /// <summary>
        /// Message displayed to user on match
        /// </summary>
        [JsonProperty("userMessage")]
        public string UserMessage { get; set; }
    }
}
