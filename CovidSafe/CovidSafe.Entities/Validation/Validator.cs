using CovidSafe.Entities.Protos;
using CovidSafe.Entities.Validation.Resources;
using System;

namespace CovidSafe.Entities.Validation
{
    /// <summary>
    /// Checks various types for validity
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Determines if a string is a valid GUID/UUID
        /// </summary>
        /// <param name="guid">String to validate</param>
        /// <param name="parameterName">Optionally, name of the parameter which originally contained the value</param>
        /// <returns><see cref="ValidationResult"/></returns>
        public static ValidationResult ValidateGuid(string guid, string parameterName = null)
        {
            ValidationResult result = new ValidationResult();

            // Seeds must parse to GUID
            Guid output;
            if (!Guid.TryParse(guid, out output))
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    parameterName ?? nameof(guid),
                    ValidationMessages.InvalidSeed,
                    guid
                );
            }

            return result;
        }

        /// <summary>
        /// Determines if a <see cref="double"/> is a valid Latitude
        /// </summary>
        /// <param name="latitude">Latitude to validate</param>
        /// <param name="parameterName">Optionally, name of the parameter which originally contained the value</param>
        /// <returns><see cref="ValidationResult"/></returns>
        public static ValidationResult ValidateLatitude(double latitude, string parameterName = null)
        {
            ValidationResult result = new ValidationResult();

            // Must be within range
            if(latitude > Location.MAX_LATITUDE || latitude < Location.MIN_LATITUDE)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    parameterName ?? nameof(latitude),
                    ValidationMessages.InvalidLatitude,
                    latitude.ToString(),
                    Location.MIN_LATITUDE.ToString(),
                    Location.MAX_LATITUDE.ToString()
                );
            }

            return result;
        }

        /// <summary>
        /// Determines if a <see cref="double"/> is a valid Longitude
        /// </summary>
        /// <param name="longitude">Longitude to validate</param>
        /// <param name="parameterName">Optionally, name of the parameter which originally contained the value</param>
        /// <returns><see cref="ValidationResult"/></returns>
        public static ValidationResult ValidateLongitude(double longitude, string parameterName = null)
        {
            ValidationResult result = new ValidationResult();

            // Must be within range
            if (longitude > Location.MAX_LONGITUDE || longitude < Location.MIN_LONGITUDE)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    parameterName ?? nameof(longitude),
                    ValidationMessages.InvalidLongitude,
                    longitude.ToString(),
                    Location.MIN_LONGITUDE.ToString(),
                    Location.MAX_LONGITUDE.ToString()
                );
            }

            return result;
        }

        /// <summary>
        /// Determines if a seed is a valid GUID/UUID
        /// </summary>
        /// <param name="seed">Seed to validate</param>
        /// <param name="parameterName">Optionally, name of the parameter which originally contained the value</param>
        /// <returns><see cref="ValidationResult"/></returns>
        public static ValidationResult ValidateSeed(string seed, string parameterName = null)
        {
            // Use ValidateGuid() given seeds are just GUID/UUIDs
            return ValidateGuid(seed, parameterName ?? nameof(seed));
        }

        /// <summary>
        /// Determines if a timestamp provided by a <see cref="long"/> is valid
        /// </summary>
        /// <param name="startTime">Starting timestamp, in ms since UNIX epoch</param>
        /// <param name="endTime">Ending timestamp, in ms since UNIX epoch</param>
        /// <returns><see cref="ValidationResult"/></returns>
        public static ValidationResult ValidateTimeRange(long startTime, long endTime)
        {
            ValidationResult result = new ValidationResult();

            // Start time must be earlier than end time
            if (startTime > endTime)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    ValidationProperty.Multiple,
                    ValidationMessages.InvalidStartEndTimeSequence,
                    startTime.ToString(),
                    endTime.ToString()
                );
            }

            return result;
        }

        /// <summary>
        /// Determines if a timestamp provided by a <see cref="long"/> is valid
        /// </summary>
        /// <param name="timestamp">Timestamp to validate, in ms since UNIX epoch</param>
        /// <param name="parameterName">Optionally, name of the parameter which originally contained the value</param>
        /// <returns><see cref="ValidationResult"/></returns>
        public static ValidationResult ValidateTimestamp(long timestamp, string parameterName = null)
        {
            ValidationResult result = new ValidationResult();

            // Timestamps must be zero or greater
            if(timestamp < 0)
            {
                result.Fail(
                    ValidationIssue.InputInvalid,
                    parameterName ?? nameof(timestamp),
                    ValidationMessages.InvalidTimestamp,
                    timestamp.ToString()
                );
            }

            return result;
        }
    }
}
