using System;

using Newtonsoft.Json;

namespace CovidSafe.Entities.Geospatial
{
    /// <summary>
    /// Defines boundaries of a <see cref="Region"/>
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [Serializable]
    public class RegionBoundary
    {
        /// <summary>
        /// Maximum <see cref="Region"/> boundary coordinates
        /// </summary>
        [JsonProperty("Max", Required = Required.Always)]
        public Coordinates Max { get; set; }
        /// <summary>
        /// Maximum <see cref="Region"/> boundary coordinates
        /// </summary>
        [JsonProperty("Min", Required = Required.Always)]
        public Coordinates Min { get; set; }

        /// <summary>
        /// Creates a new <see cref="RegionBoundary"/> instance
        /// </summary>
        public RegionBoundary()
        {
        }

        /// <summary>
        /// Creates a new <see cref="RegionBoundary"/> instance
        /// </summary>
        /// <param name="boundary">Source <see cref="RegionBoundary"/></param>
        public RegionBoundary(RegionBoundary boundary)
        {
            if (boundary != null)
            {
                this.Max = new Coordinates { Longitude = boundary.Max.Longitude, Latitude = boundary.Max.Latitude };
                this.Min = new Coordinates { Longitude = boundary.Min.Longitude, Latitude = boundary.Min.Latitude };
            }
            else
            {
                throw new ArgumentNullException(nameof(boundary));
            }
        }
    }
}