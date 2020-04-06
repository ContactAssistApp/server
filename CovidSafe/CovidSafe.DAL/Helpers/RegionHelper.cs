using System;

using CovidSafe.Entities.Protos;

namespace CovidSafe.DAL.Helpers
{
    /// <summary>
    /// Helper functions for working with <see cref="Region"/> objects
    /// </summary>
    public static class RegionHelper
    {
        /// <summary>
        /// Returns a <see cref="Region"/> from an identifier
        /// </summary>
        /// <param name="regionId">Source <see cref="Region"/> ID</param>
        /// <returns><see cref="Region"/></returns>
        public static Region FromRegionIdentifier(string regionId)
        {
            if(!String.IsNullOrEmpty(regionId))
            {
                // Parse lat/lng
                string[] parts = regionId.Split(',');
                
                if(parts.Length == 2)
                {
                    return new Region
                    {
                        LatitudePrefix = Double.Parse(parts[0]),
                        LongitudePrefix = Double.Parse(parts[1]),
                        Precision = 0
                    };
                }
                else
                {
                    throw new FormatException();
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(regionId));
            }
        }

        /// <summary>
        /// Returns a unique identifier for a provided <see cref="Area"/>
        /// </summary>
        /// <param name="area">Source <see cref="Area"/></param>
        /// <returns><see cref="Region"/> identifier</returns>
        /// <remarks>
        /// ID format strips decimal places.
        /// </remarks>
        public static string GetRegionIdentifier(Area area)
        {
            if (area != null)
            {
                return String.Format("{0},{1}", (int)area.Location.Latitude, (int)area.Location.Longitude);
            }
            else
            {
                throw new ArgumentNullException(nameof(area));
            }
        }

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
                return String.Format("{0},{1}", (int) region.LatitudePrefix, (int) region.LongitudePrefix);
            }
            else
            {
                throw new ArgumentNullException(nameof(region));
            }
        }
    }
}
