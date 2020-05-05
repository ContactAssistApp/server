using System.Collections.Generic;

using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;
using Newtonsoft.Json;

namespace CovidSafe.Entities.Reports
{
    /// <summary>
    /// Infection Report data
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class InfectionReport : IValidatable
    {
        /// <summary>
        /// Location-based infection risk reports
        /// </summary>
        [JsonProperty("areas", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public List<AreaReport> AreaReports { get; set; } = new List<AreaReport>();
        /// <summary>
        /// Message displayed to user if there is a Bluetooth match
        /// </summary>
        [JsonProperty("bluetoothMatchMessage", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public string BluetoothMatchMessage { get; set; }
        /// <summary>
        /// Bluetooth Seed-based infection reports
        /// </summary>
        [JsonProperty("seeds", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public List<BluetoothSeed> BluetoothSeeds { get; set; } = new List<BluetoothSeed>();
        /// <summary>
        /// Reserved for later use
        /// </summary>
        [JsonProperty("booleanExpression", NullValueHandling = NullValueHandling.Ignore, Required = Required.AllowNull)]
        public string BooleanExpression { get; set; }

        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Must contain at least one of either BluetoothSeeds or AreaMatches
            if (this.AreaReports.Count == 0 && this.BluetoothSeeds.Count == 0)
            {
                result.Fail(
                    RequestValidationIssue.InputEmpty,
                    RequestValidationProperty.Multiple,
                    ValidationMessages.EmptyMessage
                );
            }
            if (this.AreaReports.Count > 0)
            {
                // Validate individual area matches
                foreach (AreaReport areaReport in this.AreaReports)
                {
                    // Use AreaMatch.Validate()
                    result.Combine(areaReport.Validate());
                }
            }
            if (this.BluetoothSeeds.Count > 0)
            {
                // Validate individual Bluetooth matches
                foreach (BluetoothSeed seed in this.BluetoothSeeds)
                {
                    // Use BluetoothSeed.Validate()
                    result.Combine(seed.Validate());
                }
            }

            return result;
        }
    }
}