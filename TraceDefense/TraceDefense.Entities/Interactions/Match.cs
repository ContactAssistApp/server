using Newtonsoft.Json;

namespace TraceDefense.Entities.Interactions
{
    /// <summary>
    /// Base identifier match definition
    /// </summary>
    [JsonObject]
    public abstract class Match
    {
        /// <summary>
        /// Message displayed to user on confirmed match
        /// </summary>
        [JsonProperty("user_message")]
        public string UserMessage { get; set; }
    }
}
