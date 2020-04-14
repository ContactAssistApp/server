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

            // Timestamp must be greater than zero
            if(this.LastQueryTime < 0)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    nameof(this.LastQueryTime),
                    ValidationMessages.InvalidTimestamp,
                    this.LastQueryTime.ToString()
                );
            }

            // Use validation method from Region
            result.AddRange(this.Region.Validate());

            return result;
        }
    }
}
