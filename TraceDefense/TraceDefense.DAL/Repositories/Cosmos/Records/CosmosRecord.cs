using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace TraceDefense.DAL.Repositories.Cosmos.Records
{
    /// <summary>
    /// Cosmos Database record object
    /// </summary>
    /// <typeparam name="T">Object type to store</typeparam>
    [JsonObject]
    public abstract class CosmosRecord<T>
    {
        /// <summary>
        /// Unique <see cref="CosmosRecord{T}"/> identifier
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required]
        public string Id { get; set; }
        /// <summary>
        /// Timestamp of record database insert, in ms since UNIX epoch
        /// </summary>
        [JsonProperty("timestamp", Required = Required.Always)]
        [Required]
        public long Timestamp { get; set; }
        /// <summary>
        /// Object value
        /// </summary>
        [JsonProperty("Value", Required = Required.Always)]
        [Required]
        public T Value { get; set; }
        /// <summary>
        /// Record schema version
        /// </summary>
        [JsonProperty("version", Required = Required.Always)]
        public long Version { get; set; } = 1;
    }
}
