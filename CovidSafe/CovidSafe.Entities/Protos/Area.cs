using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;

namespace CovidSafe.Entities.Protos
{
    /// <summary>
    /// <see cref="Area"/> partial from generated Protobuf class
    /// </summary>
    public partial class Area : IValidatable
    {
        /// <inheritdoc/>
        public ValidationResult Validate()
        {
            ValidationResult result = new ValidationResult();

            // Validate location
            if(this.Location == null)
            {
                result.Fail(
                    ValidationIssue.InputNull,
                    nameof(this.Location),
                    ValidationMessages.NullLocation
                );
            }
            else
            {
                // Validate using Location.Validate()
                result.AddRange(this.Location.Validate());
            }

            // Validate timestamps
            if(this.EndTime < 0)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    nameof(this.EndTime),
                    ValidationMessages.InvalidTimestamp,
                    this.EndTime.ToString()
                );
            }
            if(this.BeginTime < 0)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    nameof(this.BeginTime),
                    ValidationMessages.InvalidTimestamp,
                    this.BeginTime.ToString()
                );
            }
            if(this.BeginTime > this.EndTime)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    ValidationProperty.Multiple,
                    ValidationMessages.InvalidStartEndTimeSequence,
                    this.BeginTime.ToString(),
                    this.EndTime.ToString()
                );
            }

            return result;
        }
    }
}
