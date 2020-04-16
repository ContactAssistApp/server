using Newtonsoft.Json;

namespace CovidSafe.Entities.Validation
{
    /// <summary>
    /// Detail of individual validation failure
    /// </summary>
    [JsonObject]
    public class ValidationFailure
    {
        /// <summary>
        /// <see cref="ValidationIssue"/> classification
        /// </summary>
        [JsonProperty("issue", Required = Required.Always)]
        public ValidationIssue Issue { get; set; }
        /// <summary>
        /// Validation failure detail message
        /// </summary>
        [JsonProperty("message", Required = Required.Always)]
        public string Message { get; set; }
        /// <summary>
        /// Name of object property which failed validation
        /// </summary>
        [JsonProperty("property", Required = Required.AllowNull)]
        public string Property { get; set; }
    }
}
