using Newtonsoft.Json;
using ProtoBuf;

namespace CovidSafe.Entities.Validation
{
    /// <summary>
    /// Detail of individual request validation failure
    /// </summary>
    [JsonObject]
    [ProtoContract]
    public class RequestValidationFailure
    {
        /// <summary>
        /// <see cref="RequestValidationIssue"/> classification
        /// </summary>
        [JsonIgnore]
        [ProtoIgnore]
        public RequestValidationIssue Issue { get; set; }
        /// <summary>
        /// Failure detail message
        /// </summary>
        [JsonProperty("message", Required = Required.Always)]
        [ProtoMember(1)]
        public string Message { get; set; }
        /// <summary>
        /// Name of object property which failed validation
        /// </summary>
        [JsonProperty("property", Required = Required.AllowNull, NullValueHandling = NullValueHandling.Ignore)]
        [ProtoMember(2)]
        public string Property { get; set; }
    }
}
