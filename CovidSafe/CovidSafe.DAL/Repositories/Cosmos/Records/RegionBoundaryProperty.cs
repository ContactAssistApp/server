using System;
using System.ComponentModel.DataAnnotations;

using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Protos;
using Microsoft.Azure.Cosmos.Spatial;
using Newtonsoft.Json;

namespace CovidSafe.DAL.Repositories.Cosmos.Records
{
    /// <summary>
    /// Adapter class mapping <see cref="RegionBoundary"/> objects to the Cosmos-provided 
    /// spatial equivalents
    /// </summary>
    [JsonObject] 
    public class RegionBoundaryProperty
    {
        /// <summary>
        /// Maximum boundary coordinate
        /// </summary>
        [JsonProperty("max", Required = Required.Always)]
        [Required]
        public Location Max { get; set; }
        /// <summary>
        /// Minimum boundary coordinate
        /// </summary>
        [JsonProperty("min", Required = Required.Always)]
        [Required]
        public Location Min { get; set; }

        /// <summary>
        /// Creates a new <see cref="RegionBoundaryProperty"/> instance
        /// </summary>
        public RegionBoundaryProperty()
        {
        }

        /// <summary>
        /// Creates a new <see cref="RegionBoundaryProperty"/> instance
        /// </summary>
        /// <param name="boundary">Source <see cref="RegionBoundary"/></param>
        public RegionBoundaryProperty(RegionBoundary boundary)
        {
            if (boundary != null)
            {
                this.Max = new Location { Latitude = boundary.Max.Longitude, Longitude = boundary.Max.Latitude };
                this.Min = new Location { Latitude = boundary.Min.Longitude, Longitude = boundary.Min.Latitude};
            }
            else
            {
                throw new ArgumentNullException(nameof(boundary));
            }
        }
    }
}
