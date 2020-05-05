using System;

using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;

namespace CovidSafe.Entities.v20200415.Protos
{
    /// <summary>
    /// <see cref="Region"/> partial from generated Protobuf class
    /// </summary>
    public partial class Region : IEquatable<Region>, global::ProtoBuf.IExtensible, IValidatable
    {
        /// <summary>
        /// Maximum allowed precision value;
        /// </summary>
        public const int MAX_PRECISION = 4;
        /// <summary>
        /// Minimum allowed precision value
        /// </summary>
        public const int MIN_PRECISION = 0;

        /// <inheritdoc/>
        public bool Equals(Region other)
        {
            return this.LatitudePrefix == other.LatitudePrefix
                && this.LongitudePrefix == other.LongitudePrefix
                && this.Precision == other.Precision;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Tuple.Create(this.LatitudePrefix, this.LongitudePrefix, this.LongitudePrefix)
                .GetHashCode();
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