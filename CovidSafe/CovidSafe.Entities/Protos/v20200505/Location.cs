using CovidSafe.Entities.Validation;

namespace CovidSafe.Entities.Protos.v20200505
{
    /// <summary>
    /// <see cref="Location"/> partial from generated Protobuf class
    /// </summary>
    public partial class Location : IValidatable
    {
        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Ensure lat/lng are within range
            result.Combine(Validator.ValidateLatitude(this.Latitude, nameof(this.Latitude)));
            result.Combine(Validator.ValidateLongitude(this.Longitude, nameof(this.Longitude)));

            return result;
        }
    }
}