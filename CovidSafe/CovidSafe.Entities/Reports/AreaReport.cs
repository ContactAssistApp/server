using System;
using System.Collections.Generic;

using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;
using Newtonsoft.Json;

namespace CovidSafe.Entities.Reports
{
    /// <summary>
    /// Area-based infection report
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AreaReport : IValidatable
    {
        /// <summary>
        /// Infection risk <see cref="InfectionArea"/> part of this <see cref="AreaReport"/>
        /// </summary>
        public List<InfectionArea> Areas { get; set; } = new List<InfectionArea>();
        /// <summary>
        /// Message displayed to user on positive match
        /// </summary>
        [JsonProperty("userMessage", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public string UserMessage { get; set; }

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
