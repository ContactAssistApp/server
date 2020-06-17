using System;

using CovidSafe.Entities.Validation;
using Newtonsoft.Json;

namespace CovidSafe.Entities.Messages
{
    /// <summary>
    /// Bluetooth seed
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [Serializable]
    public class BluetoothSeedMessage : IValidatable
    {
        /// <summary>
        /// Start of <see cref="BluetoothSeedMessage"/> validity period
        /// </summary>
        /// <remarks>
        /// Reported in milliseconds (ms) since the UNIX epoch.
        /// </remarks>
        [JsonProperty("beginTimestampMs", Required = Required.Always)]
        public long BeginTimestamp { get; set; }
        /// <summary>
        /// End of <see cref="BluetoothSeedMessage"/> validity period
        /// </summary>
        /// <remarks>
        /// Reported in milliseconds (ms) since the UNIX epoch.
        /// </remarks>
        [JsonProperty("endTimestampMs", Required = Required.Always)]
        public long EndTimestamp { get; set; }
        /// <summary>
        /// Seed backing field
        /// </summary>
        [NonSerialized]
        private string _seed;
        /// <summary>
        /// Seed value
        /// </summary>
        [JsonProperty("seed", Required = Required.Always)]
        public string Seed
        {
            get { return this._seed; }
            set { this._seed = value; }
        }

        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Seed validation
            result.Combine(Validator.ValidateSeed(this.Seed, nameof(this.Seed)));

            // Ensure times are valid
            result.Combine(Validator.ValidateTimestamp(this.BeginTimestamp, parameterName: nameof(this.BeginTimestamp)));
            result.Combine(Validator.ValidateTimestamp(this.EndTimestamp, parameterName: nameof(this.EndTimestamp)));
            result.Combine(Validator.ValidateTimeRange(this.BeginTimestamp, this.EndTimestamp));

            return result;
        }
    }
}
