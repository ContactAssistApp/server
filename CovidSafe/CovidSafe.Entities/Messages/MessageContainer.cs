using System;
using System.Collections.Generic;

using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;
using Newtonsoft.Json;

namespace CovidSafe.Entities.Messages
{
    /// <summary>
    /// Wraps all message-related types into a single, reportable object which enables mixed messages in a single result
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [Serializable]
    public class MessageContainer : IValidatable
    {
        /// <summary>
        /// BluetoothMatchMessage backing property
        /// </summary>
        [NonSerialized]
        private string _bluetoothMatchMessage;
        /// <summary>
        /// Message displayed to user if there is a Bluetooth match
        /// </summary>
        [JsonProperty("bluetoothMatchMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string BluetoothMatchMessage
        {
            get { return this._bluetoothMatchMessage; }
            set { this._bluetoothMatchMessage = value; }
        }
        /// <summary>
        /// Bluetooth Seed-based infection reports
        /// </summary>
        [JsonProperty("Seeds", NullValueHandling = NullValueHandling.Ignore)]
        public IList<BluetoothSeedMessage> BluetoothSeeds { get; set; } = new List<BluetoothSeedMessage>();
        /// <summary>
        /// BooleanExpression backing property
        /// </summary>
        [NonSerialized]
        private string _booleanExpression;
        /// <summary>
        /// Reserved for later use
        /// </summary>
        [JsonProperty("booleanExpression", NullValueHandling = NullValueHandling.Ignore)]
        public string BooleanExpression
        {
            get { return this._booleanExpression; }
            set { this._booleanExpression = value; }
        }
        /// <summary>
        /// <see cref="NarrowcastMessage"/> collection
        /// </summary>
        [JsonProperty("Narrowcasts", NullValueHandling = NullValueHandling.Ignore)]
        public IList<NarrowcastMessage> Narrowcasts { get; set; } = new List<NarrowcastMessage>();

        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Must contain at least one of either BluetoothSeeds or NarrowcastMessages
            if (this.BluetoothSeeds.Count == 0 && this.Narrowcasts.Count == 0)
            {
                result.Fail(
                    RequestValidationIssue.InputEmpty,
                    RequestValidationProperty.Multiple,
                    ValidationMessages.EmptyMessage
                );
            }

            // Validate individual messages
            if (this.BluetoothSeeds.Count > 0)
            {
                // Validate individual Bluetooth matches
                foreach (BluetoothSeedMessage seed in this.BluetoothSeeds)
                {
                    // Use BluetoothSeed.Validate()
                    result.Combine(seed.Validate());
                }
            }
            if (this.Narrowcasts.Count > 0)
            {
                // Validate individual area matches
                foreach (NarrowcastMessage message in this.Narrowcasts)
                {
                    // Use NarrowcastMessage.Validate()
                    result.Combine(message.Validate());
                }
            }

            return result;
        }
    }
}