﻿using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;

namespace CovidSafe.Entities.v20200415.Protos
{
    /// <summary>
    /// <see cref="MatchMessage"/> partial from generated Protobuf class
    /// </summary>
    public partial class MatchMessage
    {
        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Must contain at least one of either BluetoothSeeds or AreaMatches
            if (this.AreaMatches.Count == 0 && this.BluetoothMatches.Count == 0)
            {
                result.Fail(
                    RequestValidationIssue.InputEmpty,
                    RequestValidationProperty.Multiple,
                    ValidationMessages.EmptyMessage
                );
            }
            if (this.AreaMatches.Count > 0)
            {
                // Validate individual area matches
                foreach (AreaMatch areaMatch in this.AreaMatches)
                {
                    // Use AreaMatch.Validate()
                    result.Combine(areaMatch.Validate());
                }
            }
            if (this.BluetoothMatches.Count > 0)
            {
                // Validate individual Bluetooth matches
                foreach (BluetoothMatch btMatch in this.BluetoothMatches)
                {
                    // User BluetoothMatch.Validate()
                    result.Combine(btMatch.Validate());
                }
            }

            return result;
        }
    }
}