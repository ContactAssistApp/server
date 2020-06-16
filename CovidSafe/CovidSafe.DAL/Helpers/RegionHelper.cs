using System;
using System.Collections.Generic;

using CovidSafe.Entities.Geospatial;

namespace CovidSafe.DAL.Helpers
{
    /// <summary>
    /// Helper functions for working with <see cref="Region"/> objects
    /// </summary>
    public static class RegionHelper
    {
        /// <summary>
        /// Creates a new <see cref="Region"/>
        /// </summary>
        /// <param name="lat">Precise latitude</param>
        /// <param name="lng">Precise longitude</param>
        /// <remarks>
        /// This constructor allows backward-compatibility with previous 
        /// API versions which defined lat/lng as <see cref="double"/>.
        /// </remarks>
        public static Region CreateRegion(double lat, double lng)
        {
            return new Region
            {
                LatitudePrefix = PrecisionHelper.Round(lat, Region.MAX_PRECISION),
                LongitudePrefix = PrecisionHelper.Round(lng, Region.MAX_PRECISION),
                Precision = Region.MAX_PRECISION
            };
        }

        /// <summary>
        /// Creates a new <see cref="Region"/>
        /// </summary>
        /// <param name="coordinates">Geographic coordinates object</param>
        /// <remarks>
        /// When precise coordinates are provided, it is assumed to be a 
        /// full-precision Region.
        /// </remarks>
        public static Region CreateRegion(Coordinates coordinates)
        {
            return CreateRegion(coordinates.Latitude, coordinates.Longitude);
        }

        /// <summary>
        /// Creates a new <see cref="RegionBoundary"/> from a provided <see cref="Region"/>
        /// </summary>
        /// <param name="region">Source <see cref="Region"/></param>
        public static RegionBoundary GetRegionBoundary(Region region)
        {
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            return new RegionBoundary
            {
                Min = new Coordinates { Latitude = 0, Longitude = 0 },
                Max = new Coordinates { Latitude = 0, Longitude = 0 }
            };
        }

        /// <summary>
        /// Adjusts region coordinate prefixes to be aligned with precision<see cref="Region"/>/>
        /// </summary>
        /// <param name="region">Input <see cref="Region"/></param>
        /// <returns><see cref="Region"/> - region with adjusted coordinate prefixes</returns>
        public static Region AdjustToPrecision(Region region)
        {
            return new Region(
                PrecisionHelper.Round(region.LatitudePrefix, region.Precision),
                PrecisionHelper.Round(region.LongitudePrefix, region.Precision),
                region.Precision
            );
        }

        /// <summary>
        /// Enumerates all <see cref="Region"/>s of given precision covering given area/>
        /// Current implementation only returns single region associated with area center.
        /// Under assumption of radius of area to be significantly below precision resolution and usage of extension >= 1 in search,
        /// that is enough
        /// </summary>
        /// <param name="area"><see cref="NarrowcastArea"/></param>
        /// <param name="precision">Precision. Any integer number</param>
        /// <returns>Collection of <see cref="Region"/>s</returns>

        public static IEnumerable<Region> GetRegionsCoverage(NarrowcastArea area, int precision)
        {
            yield return new Region(
                PrecisionHelper.Round(area.Location.Latitude, precision),
                PrecisionHelper.Round(area.Location.Longitude, precision),
                precision
            );
        }

        /// <summary>
        /// Enumerates all <see cref="Region"/>s of given precision covering given areas/>
        /// </summary>
        /// <param name="areas">Container of <see cref="NarrowcastArea"/>s</param>
        /// <param name="precision">Precision. Any integer number</param>
        /// <returns>Collection of <see cref="Region"/>s</returns>

        public static IEnumerable<Region> GetRegionsCoverage(IEnumerable<NarrowcastArea> areas, int precision)
        {
            var result = new HashSet<Region>(new RegionComparer());
            foreach (var a in areas)
            {
                result.UnionWith(GetRegionsCoverage(a, precision));
            }
            return result;
        }
    }
}