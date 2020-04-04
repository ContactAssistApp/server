using System;
using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Protos;

namespace CovidSafe.DAL.Helpers
{
    /// <summary>
    /// Helper functions for working with <see cref="Region"/> objects
    /// </summary>
    public static class RegionHelper
    {
        /// <summary>
        /// Returns a unique identifier for a provided <see cref="Region"/>
        /// </summary>
        /// <param name="region">Source <see cref="Region"/></param>
        /// <returns><see cref="Region"/> identifier</returns>
        /// <remarks>
        /// ID format strips decimal places.
        /// </remarks>
        public static string GetRegionIdentifier(Region region)
        {
            if(region != null)
            {
                return String.Format("{0},{1}", (int) region.LattitudePrefix, (int) region.LongitudePrefix);
            }
            else
            {
                throw new ArgumentNullException(nameof(region));
            }
        }

        /// <summary>
        /// Creates a new <see cref="RegionBoundary"/> from a provided <see cref="Region"/>
        /// </summary>
        /// <param name="region">Source <see cref="Region"/></param>
        /// <remarks>
        /// TODO: Create actual logic for this.
        /// </remarks>
        public static RegionBoundary GetRegionBoundary(Region region)
        {
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            Tuple<double, double> latRange = PrecisionHelper.GetRange(region.LattitudePrefix, region.Precision);
            Tuple<double, double> lonRange = PrecisionHelper.GetRange(region.LongitudePrefix, region.Precision);

            return new RegionBoundary
            {
                Min = new Location { Lattitude = latRange.Item1, Longitude = lonRange.Item1 },
                Max = new Location { Lattitude = latRange.Item2, Longitude = lonRange.Item2 }
            };
        }
    }
}
