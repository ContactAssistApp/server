using System;

using CovidSafe.Entities.Validation;
using Newtonsoft.Json;

namespace CovidSafe.Entities.Geospatial
{
    /// <summary>
    /// Geographic location coordinates
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [Serializable]
    public class Coordinates : IValidatable
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

        /// <summary>
        /// Latitude of coordinate
        /// </summary>
        [JsonProperty("lat", Required = Required.Always)]
        public double Latitude { get; set; }
        /// <summary>
        /// Longitude of coordinate
        /// </summary>
        [JsonProperty("lng", Required = Required.Always)]
        public double Longitude { get; set; }

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