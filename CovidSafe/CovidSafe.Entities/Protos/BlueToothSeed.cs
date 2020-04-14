using System;

using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;

namespace CovidSafe.Entities.Protos
{
    /// <summary>
    /// <see cref="BlueToothSeed"/> partial from generated Protobuf class
    /// </summary>
    public partial class BlueToothSeed
    {
        /// <inheritdoc/>
        public ValidationResult Validate()
        {
            ValidationResult result = new ValidationResult();

            // Seed must be GUID/UUID type
            Guid output;
            if(!Guid.TryParse(this.Seed, out output))
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    nameof(this.Seed),
                    ValidationMessages.InvalidSeed,
                    this.Seed
                );
            }
            // Ensure times are valid
            if(this.SequenceEndTime < 0)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    nameof(this.SequenceEndTime),
                    ValidationMessages.InvalidTimestamp,
                    this.SequenceEndTime.ToString()
                );
            }
            if(this.SequenceStartTime < 0)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    nameof(this.SequenceStartTime),
                    ValidationMessages.InvalidTimestamp,
                    this.SequenceStartTime.ToString()
                );
            }
            if(this.SequenceStartTime > this.SequenceEndTime)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    ValidationProperty.Multiple,
                    ValidationMessages.InvalidStartEndTimeSequence,
                    this.SequenceStartTime.ToString(),
                    this.SequenceEndTime.ToString()
                );
            }

            return result;
        }
    }
}
