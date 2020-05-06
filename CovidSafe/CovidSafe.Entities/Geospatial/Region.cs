using System;

using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;
using Newtonsoft.Json;

namespace CovidSafe.Entities.Geospatial
{
    /// <summary>
    /// Geographic region
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [Serializable]
    public class Region : IValidatable
    {
        /// <summary>
        /// Maximum allowed precision value;
        /// </summary>
        public const int MAX_PRECISION = 4;
        /// <summary>
        /// Minimum allowed precision value
        /// </summary>
        public const int MIN_PRECISION = 0;

        /// <summary>
        /// Latitude prefix of this <see cref="Region"/>
        /// </summary>
        [JsonProperty("latPrefix", Required = Required.Always)]
        public double LatitudePrefix { get; set; }
        /// <summary>
        /// Longitude prefix of this <see cref="Region"/>
        /// </summary>
        [JsonProperty("lngPrefix", Required = Required.Always)]
        public double LongitudePrefix { get; set; }
        /// <summary>
        /// Precision of this <see cref="Region"/>
        /// </summary>
        /// <remarks>
        /// Used as a mantissa mask.
        /// </remarks>
        [JsonProperty("precision", Required = Required.Always)]
        public int Precision { get; set; }

        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Check if lat/lng are in expected values
            result.Combine(Validator.ValidateLatitude(this.LatitudePrefix, nameof(this.LatitudePrefix)));
            result.Combine(Validator.ValidateLongitude(this.LongitudePrefix, nameof(this.LongitudePrefix)));

            // Validate precision
            if (this.Precision < MIN_PRECISION || this.Precision > MAX_PRECISION)
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
