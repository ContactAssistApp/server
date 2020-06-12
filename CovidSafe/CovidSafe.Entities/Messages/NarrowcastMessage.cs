using System;

using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;
using Newtonsoft.Json;

namespace CovidSafe.Entities.Messages
{
    /// <summary>
    /// Narrowcast message container
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [Serializable]
    public class NarrowcastMessage : IValidatable
    {
        /// <summary>
        /// <see cref="NarrowcastArea"/> targeted by this <see cref="NarrowcastMessage"/>
        /// </summary>
        [JsonProperty("Area", Required = Required.Always)]
        public NarrowcastArea Area { get; set; }
        /// <summary>
        /// Internal UserMessage backing field
        /// </summary>
        [NonSerialized]
        private string _userMessage;
        /// <summary>
        /// Message displayed to user on positive match
        /// </summary>
        [JsonProperty("userMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string UserMessage
        {
            get { return this._userMessage; }
            set { _userMessage = value; }
        }

        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Validate provided NarrowcastArea
            if (this.Area != null)
            {
                // Use NarrowcastArea.Validate()
                result.Combine(this.Area.Validate());
            }
            else
            {
                result.Fail(
                    RequestValidationIssue.InputEmpty,
                    nameof(this.Area),
                    ValidationMessages.EmptyAreas
                );
            }

            // Validate message
            if (String.IsNullOrEmpty(this.UserMessage))
            {
                result.Fail(
                    RequestValidationIssue.InputEmpty,
                    nameof(this.UserMessage),
                    ValidationMessages.EmptyMessage
                );
            }

            return result;
        }
    }
}
