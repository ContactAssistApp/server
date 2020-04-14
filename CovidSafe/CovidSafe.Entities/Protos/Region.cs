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
        public const int MAX_PRECISION = 8;
        /// <summary>
        /// Minimum allowed precision value
        /// </summary>
        public const int MIN_PRECISION = 0;

        /// <inheritdoc/>
        public ValidationResult Validate()
        {
            ValidationResult result = new ValidationResult();

            // Check if lat/lng are in expected values
            if(this.LatitudePrefix > Location.MAX_LATITUDE || this.LatitudePrefix < Location.MIN_LATITUDE)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    nameof(this.LatitudePrefix),
                    ValidationMessages.InvalidLatitude,
                    this.LatitudePrefix.ToString(),
                    Location.MIN_LATITUDE.ToString(),
                    Location.MAX_LATITUDE.ToString()
                );
            }
            if (this.LongitudePrefix > Location.MAX_LONGITUDE || this.LongitudePrefix < Location.MIN_LONGITUDE)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    nameof(this.LongitudePrefix),
                    ValidationMessages.InvalidLongitude,
                    this.LongitudePrefix.ToString(),
                    Location.MIN_LONGITUDE.ToString(),
                    Location.MAX_LONGITUDE.ToString()
                );
            }
            // Validate precision
            if(this.Precision < MIN_PRECISION || this.Precision > MAX_PRECISION)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
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
