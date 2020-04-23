using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;

namespace CovidSafe.Entities.Protos
{
    /// <summary>
    /// <see cref="Region"/> partial from generated Protobuf class
    /// </summary>
    public partial class Region : global::ProtoBuf.IExtensible, IValidatable
    {
        /// <summary>
        /// Maximum allowed precision value;
        /// </summary>
        public const int MAX_PRECISION = 4;
        /// <summary>
        /// Minimum allowed precision value
        /// </summary>
        public const int MIN_PRECISION = 4;

        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Check if lat/lng are in expected values
            result.Combine(Validator.ValidateLatitude(this.LatitudePrefix, nameof(this.LatitudePrefix)));
            result.Combine(Validator.ValidateLongitude(this.LongitudePrefix, nameof(this.LongitudePrefix)));

            // Validate precision
            if(this.Precision < MIN_PRECISION || this.Precision > MAX_PRECISION)
            {
                result.Fail(
                    RequestValidationIssue.InputInvalid,
                    nameof(this.Precision),
                    ValidationMessages.InvalidPrecision,
                    this.Precision.ToString(),
                    MIN_PRECISION.ToString(),
                    MAX_PRECISION.ToString()
                );
            }

            return result;
        }
    }
}
