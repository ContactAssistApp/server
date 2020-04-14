using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;

namespace CovidSafe.Entities.Protos
{
    /// <summary>
    /// <see cref="Location"/> partial from generated Protobuf class
    /// </summary>
    public partial class Location : IValidatable
    {
        /// <summary>
        /// Maximum allowable latitude
        /// </summary>
        public const double MAX_LATITUDE = 90;
        /// <summary>
        /// Maximum allowable longitude
        /// </summary>
        public const double MAX_LONGITUDE = 180;
        /// <summary>
        /// Minimum allowable latitude
        /// </summary>
        public const double MIN_LATITUDE = -90;
        /// <summary>
        /// Minimum allowable longitude
        /// </summary>
        public const double MIN_LONGITUDE = -180;

        /// <inheritdoc/>
        public ValidationResult Validate()
        {
            ValidationResult result = new ValidationResult();

            // Ensure lat/lng are within range
            if(this.Latitude > MAX_LATITUDE || this.Latitude < MIN_LATITUDE)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    nameof(this.Latitude),
                    ValidationMessages.InvalidLatitude,
                    this.Latitude.ToString(),
                    MIN_LATITUDE.ToString(),
                    MAX_LATITUDE.ToString()
                );
            }
            if(this.Longitude > MAX_LONGITUDE || this.Longitude < MIN_LONGITUDE)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    nameof(this.Longitude),
                    ValidationMessages.InvalidLongitude,
                    this.Longitude.ToString(),
                    MIN_LONGITUDE.ToString(),
                    MAX_LONGITUDE.ToString()
                );
            }

            return result;
        }
    }
}