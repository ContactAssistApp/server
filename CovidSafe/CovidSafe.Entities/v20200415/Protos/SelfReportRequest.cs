using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;

namespace CovidSafe.Entities.v20200505.Protos
{
    /// <summary>
    /// <see cref="SelfReportRequest"/> partial from generated Protobuf class
    /// </summary>
    public partial class SelfReportRequest : IValidatable
    {
        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Only validate if collection contains message information
            if (this.Seeds == null || this.Seeds.Count == 0)
            {
                result.Fail(
                    RequestValidationIssue.InputEmpty,
                    nameof(this.Seeds),
                    ValidationMessages.EmptySeeds
                );
            }
            else
            {
                // Validate each seed
                foreach (BlueToothSeed seed in this.Seeds)
                {
                    // Use BlueToothSeed.Validate()
                    result.Combine(seed.Validate());
                }
            }

            // Validate Region
            if (this.Region == null)
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