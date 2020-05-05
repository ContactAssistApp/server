using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;
using Newtonsoft.Json;

namespace CovidSafe.Entities.Geospatial
{
    /// <summary>
    /// Space and time of infection risk zone
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class InfectionArea : IValidatable
    {
        /// <summary>
        /// Start time of infection risk period
        /// </summary>
        /// <remarks>
        /// Reported in milliseconds (ms) since the UNIX epoch.
        /// </remarks>
        [JsonProperty("beginTimestampMs", Required = Required.Always)]
        public long BeginTimestamp { get; set; }
        /// <summary>
        /// End time of infection risk period
        /// </summary>
        /// <remarks>
        /// Reported in milliseconds (ms) since the UNIX epoch.
        /// </remarks>
        [JsonProperty("endTimestampMs", Required = Required.Always)]
        public long EndTimestamp { get; set; }
        /// <summary>
        /// Geographic coordinates of the center of the infection risk zone
        /// </summary>
        [JsonProperty("location", Required = Required.Always)]
        public Coordinates Location { get; set; }
        /// <summary>
        /// Radius of infection risk area
        /// </summary>
        /// <remarks>
        /// Reported in meters (m)
        /// </remarks>
        [JsonProperty("radiusMeters", Required = Required.Always)]
        public float RadiusMeters { get; set; }

        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Validate location
            if (this.Location == null)
            {
                result.Fail(
                    RequestValidationIssue.InputNull,
                    nameof(this.Location),
                    ValidationMessages.NullLocation
                );
            }
            else
            {
                // Validate using Coordinates.Validate()
                result.Combine(this.Location.Validate());
            }

            // Validate timestamps
            result.Combine(Validator.ValidateTimestamp(this.BeginTimestamp, parameterName: nameof(this.BeginTimestamp)));
            result.Combine(Validator.ValidateTimestamp(this.EndTimestamp, parameterName: nameof(this.EndTimestamp)));
            result.Combine(Validator.ValidateTimeRange(this.BeginTimestamp, this.EndTimestamp));

            return result;
        }
    }
}
