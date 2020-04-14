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
                result.Combine(this.Location.Validate());
            }

            // Validate timestamps
            result.Combine(Validator.ValidateTimestamp(this.BeginTime, nameof(this.BeginTime)));
            result.Combine(Validator.ValidateTimestamp(this.EndTime, nameof(this.EndTime)));
            result.Combine(Validator.ValidateTimeRange(this.BeginTime, this.EndTime));

            return result;
        }
    }
}
