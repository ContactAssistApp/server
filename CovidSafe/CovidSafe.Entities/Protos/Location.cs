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
            result.Combine(Validator.ValidateLatitude(this.Latitude, nameof(this.Latitude)));
            result.Combine(Validator.ValidateLongitude(this.Longitude, nameof(this.Longitude)));

            return result;
        }
    }
}