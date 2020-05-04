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
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Validate location
            if(this.Location == null)
            {
                result.Fail(
                    RequestValidationIssue.InputNull,
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
            result.Combine(Validator.ValidateTimestamp(this.BeginTime, parameterName: nameof(this.BeginTime)));
            result.Combine(Validator.ValidateTimestamp(this.EndTime, parameterName: nameof(this.EndTime)));
            result.Combine(Validator.ValidateTimeRange(this.BeginTime, this.EndTime));

            return result;
        }
    }
}
