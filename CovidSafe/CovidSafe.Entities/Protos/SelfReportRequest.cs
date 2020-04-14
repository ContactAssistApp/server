using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;

namespace CovidSafe.Entities.Protos
{
    /// <summary>
    /// <see cref="SelfReportRequest"/> partial from generated Protobuf class
    /// </summary>
    public partial class SelfReportRequest : IValidatable
    {
        /// <inheritdoc/>
        public ValidationResult Validate()
        {
            ValidationResult result = new ValidationResult();

            // Only validate if collection contains message information
            if(this.Seeds == null || this.Seeds.Count == 0)
            {
                result.Fail(
                    ValidationIssue.InputEmpty,
                    nameof(this.Seeds),
                    ValidationMessages.EmptySeeds
                );
            }
            else
            {
                // Validate each seed
                foreach(BlueToothSeed seed in this.Seeds)
                {
                    // Use BlueToothSeed.Validate()
                    result.Combine(seed.Validate());
                }
            }

            // Validate timestamp
            result.Combine(Validator.ValidateTimestamp(this.ClientTimestamp, nameof(this.ClientTimestamp)));

            // Validate Region
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
