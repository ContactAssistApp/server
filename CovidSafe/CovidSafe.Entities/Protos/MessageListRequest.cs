using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;

namespace CovidSafe.Entities.Protos
{
    /// <summary>
    /// <see cref="MessageListRequest"/> partial from generated Protobuf class
    /// </summary>
    public partial class MessageListRequest : IValidatable
    {
        /// <inheritdoc/>
        public ValidationResult Validate()
        {
            ValidationResult result = new ValidationResult();

            // Validate timestamp
            result.Combine(Validator.ValidateTimestamp(this.LastQueryTime, nameof(this.LastQueryTime)));
            
            // Validate region
            if(this.Region == null)
            {
                result.Fail(
                    ValidationIssue.InputNull,
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
