using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;

namespace CovidSafe.Entities.v20200505.Protos
{
    /// <summary>
    /// <see cref="MessageListRequest"/> partial from generated Protobuf class
    /// </summary>
    public partial class MessageListRequest : IValidatable
    {
        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Validate timestamp
            result.Combine(Validator.ValidateTimestamp(this.LastQueryTime, parameterName: nameof(this.LastQueryTime)));

            // Validate region
            if(this.Region == null)
            {
                result.Fail(
                    RequestValidationIssue.InputNull,
                    nameof(this.Region),
                    ValidationMessages.NullRegion
                );
            }
            else
            {
                // Use Region.Validate()
                result.Combine(this.Region.Validate());
            }

            return result;
        }
    }
}
