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
        /// Maximum allowed age of a timestamp, in days
        /// </summary>
        public const int MAX_TIMESTAMP_AGE_DAYS = 28;

        /// <summary>
        /// Determines if a string is a valid GUID/UUID
        /// </summary>
        /// <param name="guid">String to validate</param>
        /// <param name="parameterName">Optionally, name of the parameter which originally contained the value</param>
        /// <returns><see cref="RequestValidationResult"/></returns>
        public static RequestValidationResult ValidateGuid(string guid, string parameterName = null)
        {
            RequestValidationResult result = new RequestValidationResult();

            // Seeds must parse to GUID
            Guid output;
            if (!Guid.TryParse(guid, out output))
            {
                result.Fail(
                    RequestValidationIssue.InputInvalid,
                    parameterName ?? nameof(guid),
                    ValidationMessages.InvalidSeed,
                    guid
                );
            }
            // Guid will be normalized already, so only need to check for this all-zero case
            else if (output.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                result.Fail(
                    RequestValidationIssue.InputInvalid,
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
        /// <returns><see cref="RequestValidationResult"/></returns>
        public static RequestValidationResult ValidateLatitude(double latitude, string parameterName = null)
        {
            RequestValidationResult result = new RequestValidationResult();

            // Must be within range
            if(latitude > Location.MAX_LATITUDE || latitude < Location.MIN_LATITUDE)
            {
                result.Fail(
                    RequestValidationIssue.InputInvalid,
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
        /// <returns><see cref="RequestValidationResult"/></returns>
        public static RequestValidationResult ValidateLongitude(double longitude, string parameterName = null)
        {
            RequestValidationResult result = new RequestValidationResult();

            // Must be within range
            if (longitude > Location.MAX_LONGITUDE || longitude < Location.MIN_LONGITUDE)
            {
                result.Fail(
                    RequestValidationIssue.InputInvalid,
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
        /// <returns><see cref="RequestValidationResult"/></returns>
        public static RequestValidationResult ValidateSeed(string seed, string parameterName = null)
        {
            // Use ValidateGuid() given seeds are just GUID/UUIDs
            return ValidateGuid(seed, parameterName ?? nameof(seed));
        }

        /// <summary>
        /// Determines if a timestamp provided by a <see cref="long"/> is valid
        /// </summary>
        /// <param name="startTime">Starting timestamp, in ms since UNIX epoch</param>
        /// <param name="endTime">Ending timestamp, in ms since UNIX epoch</param>
        /// <returns><see cref="RequestValidationResult"/></returns>
        public static RequestValidationResult ValidateTimeRange(long startTime, long endTime)
        {
            RequestValidationResult result = new RequestValidationResult();

            // Start time must be earlier than end time
            if (startTime > endTime)
            {
                result.Fail(
                    RequestValidationIssue.InputInvalid,
                    RequestValidationProperty.Multiple,
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
        /// <param name="asOf">Timestamp (in ms since UNIX epoch) used to determine age, which defaults to current time in UTC if '0'</param>
        /// <param name="maxAgeDays">Maximum allowed age of timestamp, in minutes</param>
        /// <param name="parameterName">Optionally, name of the parameter which originally contained the value</param>
        /// <returns><see cref="RequestValidationResult"/></returns>
        public static RequestValidationResult ValidateTimestamp(long timestamp, long asOf = 0, int maxAgeDays = MAX_TIMESTAMP_AGE_DAYS, string parameterName = null)
        {
            RequestValidationResult result = new RequestValidationResult();

            DateTimeOffset baseTimestamp;

            if(asOf < 0)
            {
                throw new ArgumentException(nameof(asOf));
            }
            else if(asOf == 0)
            {
                // Set to current timestamp in UTC
                baseTimestamp = DateTimeOffset.UtcNow;
            }
            else
            {
                baseTimestamp = DateTimeOffset.FromUnixTimeMilliseconds(asOf);
            }

            // Timestamps must be zero or greater
            if(timestamp < 0)
            {
                result.Fail(
                    RequestValidationIssue.InputInvalid,
                    parameterName ?? nameof(timestamp),
                    ValidationMessages.InvalidTimestamp,
                    timestamp.ToString()
                );
            }
            else
            {
                // Calculate age boundaries
                long minAgeMs = baseTimestamp.AddDays(-(maxAgeDays)).ToUnixTimeMilliseconds();
                long maxAgeMs = baseTimestamp.AddDays(1).ToUnixTimeMilliseconds();

                if(timestamp > maxAgeMs || timestamp < minAgeMs)
                {
                    result.Fail(
                        RequestValidationIssue.InputInvalid,
                        parameterName ?? nameof(timestamp),
                        ValidationMessages.InvalidTimestampAge,
                        timestamp.ToString()
                    );
                }
            }

            return result;
        }
    }
}
