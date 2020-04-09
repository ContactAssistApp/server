using System;

using CovidSafe.Entities.Protos;

namespace CovidSafe.Entities.Geospatial
{
    /// <summary>
    /// Defines boundaries of a <see cref="Region"/>
    /// </summary>
    public class RegionBoundary
    {
        /// <summary>
        /// Maximum <see cref="Region"/> boundary coordinates
        /// </summary>
        public Location Max { get; set; }
        /// <summary>
        /// Maximum <see cref="Region"/> boundary coordinates
        /// </summary>
        public Location Min { get; set; }
    }
}
