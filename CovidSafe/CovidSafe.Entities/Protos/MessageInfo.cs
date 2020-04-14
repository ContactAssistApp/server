using System;

using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;

namespace CovidSafe.Entities.Protos
{
    /// <summary>
    /// <see cref="MessageInfo"/> partial from generated Protobuf class
    /// </summary>
    public partial class MessageInfo : IValidatable
    {
        /// <inheritdoc/>
        public ValidationResult Validate()
        {
            ValidationResult result = new ValidationResult();

            // MessageId must be a GUID/UUID
            Guid output;
            if(!Guid.TryParse(this.MessageId, out output))
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    nameof(this.MessageId),
                    ValidationMessages.InvalidGuid,
                    this.MessageId
                );
            }
            // Timestamp validation
            if(this.MessageTimestamp < 0)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    nameof(this.MessageTimestamp),
                    ValidationMessages.InvalidTimestamp,
                    this.MessageTimestamp.ToString()
                );
            }

            return result;
        }
    }
}
