using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace CovidSafe.DAL.Repositories.Cosmos.Records
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
        /// <remarks>
        /// Do NOT set this to 'Required'. Let CosmosDB auto-generate this.
        /// </remarks>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// Partition Key value
        /// </summary>
        [JsonProperty("partitionKey", Required = Required.Always)]
        [Required]
        public string PartitionKey { get; set; }
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
        public string Version { get; set; } = "";

        /// <summary>
        /// Creates a new <see cref="CosmosRecord{T}"/> instance
        /// </summary>
        public CosmosRecord()
        {
            // Set default local values
            this.Id = Guid.NewGuid().ToString();
            this.PartitionKey = GetPartitionKey();
            this.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Generates a new Partition Key value for the record
        /// </summary>
        /// <param name="timestamp">Timestamp used to derive Partition Key</param>
        /// <returns><see cref="PartitionKey"/></returns>
        public virtual string GetPartitionKey(DateTimeOffset? timestamp = null)
        {
            if (!timestamp.HasValue)
            {
                // UtcNow is default
                timestamp = DateTimeOffset.UtcNow;
            }

            // Generate key based on the current day, in UNIX epoch
            DateTimeOffset value = new DateTimeOffset(
                timestamp.Value.Year,
                timestamp.Value.Month,
                timestamp.Value.Day,
                0,
                0,
                0,
                TimeSpan.Zero
            );

            // Return as UNIX epoch, in ms
            return value.ToUnixTimeMilliseconds().ToString();
        }
    }
}
