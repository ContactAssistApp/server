using System;
using System.Collections.Generic;

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
        /// Infection risk <see cref="InfectionArea"/> part of this <see cref="NarrowcastMessage"/>
        /// </summary>
        [JsonProperty("Areas", Required = Required.Always)]
        public IList<InfectionArea> Areas { get; set; } = new List<InfectionArea>();
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

            // Validate areas
            if (this.Areas.Count > 0)
            {
                // Validate individual areas
                foreach (InfectionArea area in this.Areas)
                {
                    // Use Area.Validate()
                    result.Combine(area.Validate());
                }
            }
            else
            {
                result.Fail(
                    RequestValidationIssue.InputEmpty,
                    nameof(this.Areas),
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
