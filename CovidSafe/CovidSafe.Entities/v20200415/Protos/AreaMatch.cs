using System;

using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;

namespace CovidSafe.Entities.v20200415.Protos
{
    /// <summary>
    /// <see cref="AreaMatch"/> partial from generated Protobuf class
    /// </summary>
    public partial class AreaMatch : IValidatable
    {
        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Validate areas
            if (this.Areas.Count > 0)
            {
                // Validate individual areas
                foreach (Area area in this.Areas)
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