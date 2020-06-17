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
        public const int MAX_PRECISION = 8;
        /// <summary>
        /// Minimum allowed precision value
        /// </summary>
        public const int MIN_PRECISION = 0;
        /// <summary>
        /// Latitude prefix of this <see cref="Region"/>
        /// </summary>
        /// <remarks>
        /// A region lat/lng should not have decimals, to avoid unmasking a user 
        /// while getting the necessary amount of information to provide 
        /// accurate messaging.
        /// </remarks>
        [JsonProperty("latPrefix", Required = Required.Always)]
        public int LatitudePrefix { get; set; }
        /// <summary>
        /// Longitude prefix of this <see cref="Region"/>
        /// </summary>
        /// <remarks>
        /// A region lat/lng should not have decimals, to avoid unmasking a user 
        /// while getting the necessary amount of information to provide 
        /// accurate messaging.
        /// </remarks>
        [JsonProperty("lngPrefix", Required = Required.Always)]
        public int LongitudePrefix { get; set; }
        /// <summary>
        /// Precision of this <see cref="Region"/>
        /// </summary>
        /// <remarks>
        /// Used as a mantissa mask. The maximum available precision value is 
        /// 9, which should be assumed in most cases when a direct latitude or 
        /// longitude is provided.
        /// </remarks>
        [JsonProperty("precision", Required = Required.Always)]
        public int Precision { get; set; } = MAX_PRECISION;

        /// <summary>
        /// Default <see cref="Region"/> constructor
        /// </summary>
        public Region() { }

        /// <summary>
        /// Creates a new <see cref="Region"/>
        /// </summary>
        /// <param name="latPrefix">Latitude prefix</param>
        /// <param name="lngPrefix">Longitude prefix</param>
        /// <param name="precision">Precision value</param>
        public Region(int latPrefix, int lngPrefix, int precision = MAX_PRECISION)
        {
            this.LatitudePrefix = latPrefix;
            this.LongitudePrefix = lngPrefix;
            this.Precision = precision;
        }

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
