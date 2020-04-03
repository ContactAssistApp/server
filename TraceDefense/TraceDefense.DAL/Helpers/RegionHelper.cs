using System;

using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Helpers
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
        public static string GetRegionIdentifier(Region region)
        {
            return String.Format("{0},{1}", region.LattitudePrefix, region.LongitudePrefix);
        }
    }
}
