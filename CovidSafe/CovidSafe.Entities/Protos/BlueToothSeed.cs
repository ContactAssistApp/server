using CovidSafe.Entities.Validation;

namespace CovidSafe.Entities.Protos
{
    /// <summary>
    /// <see cref="BlueToothSeed"/> partial from generated Protobuf class
    /// </summary>
    public partial class BlueToothSeed
    {
        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Seed validation
            result.Combine(Validator.ValidateSeed(this.Seed, nameof(this.Seed)));

            // Ensure times are valid
            result.Combine(Validator.ValidateTimestamp(this.SequenceStartTime, nameof(this.SequenceStartTime)));
            result.Combine(Validator.ValidateTimestamp(this.SequenceEndTime, nameof(this.SequenceEndTime)));
            result.Combine(Validator.ValidateTimeRange(this.SequenceStartTime, this.SequenceEndTime));

            return result;
        }
    }
}
