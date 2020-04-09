using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        /// Creates a new <see cref="RegionBoundary"/> from a provided <see cref="Region"/>
        /// </summary>
        /// <param name="region">Source <see cref="Region"/></param>
        public static RegionBoundary GetRegionBoundary(Region region)
        {
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            Tuple<double, double> latRange = PrecisionHelper.GetRange(region.LatitudePrefix, region.Precision);
            Tuple<double, double> lonRange = PrecisionHelper.GetRange(region.LongitudePrefix, region.Precision);

            return new RegionBoundary
            {
                Min = new Location { Latitude = latRange.Item1, Longitude = lonRange.Item1 },
                Max = new Location { Latitude = latRange.Item2, Longitude = lonRange.Item2 }
            };
        }

        /// <summary>
        /// Enumerates all regions of given precisions range connected with given <see cref="Region"/>/>
        /// Being connected means either intersects with the region extended by overlap amount of precision-aligned grids
        /// </summary>
        /// <param name="region">Source <see cref="Region"/></param>
        /// <param name="extension">Size of region extension (in precision-aligned steps)</param>
        /// <param name="precisionStart"> Start precision parameter. Any integer value.</param>
        /// <param name="precisionCount"> Count of precision parameters to include. Any non-negative integer value.</param>
        /// <returns>Collection of connected <see cref="Region"/> objects</returns>
        public static IEnumerable<Region> GetConnectedRegions(Region region, int extension, int precisionStart, int precisionCount = 1)
        {
            RegionBoundary rb = GetRegionBoundary(region);
            for (int precision = precisionStart; precision < precisionStart + precisionCount; ++precision)
            {
                double step = PrecisionHelper.GetStep(precision);
                for (double lat = rb.Min.Latitude - extension * step; lat < rb.Max.Latitude + extension * step; lat += step)
                {
                    for (double lon = rb.Min.Longitude - extension * step; lon < rb.Max.Longitude + extension * step; lon += step)
                    {
                        yield return new Region
                        {
                            LatitudePrefix = lat,
                            LongitudePrefix = lon,
                            Precision = precision
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Adjusts region coordinate prefixes to be aligned with precision<see cref="Region"/>/>
        /// </summary>
        /// <param name="region">Input <see cref="Region"/></param>
        /// <returns><see cref="Region"/> - region with adjusted coordinate prefixes</returns>
        public static Region AdjustToPrecision(Region region)
        {
            return new Region
            {
                LatitudePrefix = PrecisionHelper.Round(region.LatitudePrefix, region.Precision),
                LongitudePrefix = PrecisionHelper.Round(region.LongitudePrefix, region.Precision),
                Precision = region.Precision
            };
        }

        ///<summary>
        ///For given region, target precision and extension size,
        ///returns region boundary that gives available ranges of latitude/longitude 
        ///for regions of target precision, overlapping with extension of given region
        /// </summary>
        /// <param name="region">Source <see cref="Region"/></param>
        /// <param name="extension">Size of region extension (in precision-aligned steps)</param>
        /// <param name="precision"> Target precision parameter. Any integer value.</param>
        /// <returns>Region boundary rb. Every region with Lat/Lon of corresponding RegionBoundary's min
        /// belonging to rb overlaps with extension of given region <see cref="Region"/> objects</returns>
        public static RegionBoundary GetConnectedRegionsRange(Region region, int extension, int precision)
        {
            RegionBoundary rb = RegionHelper.GetRegionBoundary(region);

            double step = PrecisionHelper.GetStep(precision);
            rb.Min.Latitude -= extension * step;
            rb.Min.Longitude -= extension * step;

            rb.Max.Latitude += (extension - 1) * step;
            rb.Max.Latitude += (extension - 1) * step;

            return rb;
        }

        /// <summary>
        /// Enumerates all <see cref="Region"/>s of given precision covering given area/>
        /// Current implementation only returns single region associated with area center.
        /// Under assumption of radius of area to be significantly below precision resolution and usage of extension >= 1 in search,
        /// that is enough
        /// </summary>
        /// <param name="area"><see cref="Area"/></param>
        /// <param name="precision">Precision. Any integer number</param>
        /// <returns>Collection of <see cref="Region"/>s</returns>

        public static IEnumerable<Region> GetRegionsCoverage(Area area, int precision)
        {
            yield return new Region
            {
                LatitudePrefix = PrecisionHelper.Round(area.Location.Latitude, precision),
                LongitudePrefix = PrecisionHelper.Round(area.Location.Longitude, precision),
                Precision = precision
            };
        }

        /// <summary>
        /// Enumerates all <see cref="Region"/>s of given precision covering given areas/>
        /// </summary>
        /// <param name="areas">Container of <see cref="Area"/>s</param>
        /// <param name="precision">Precision. Any integer number</param>
        /// <returns>Collection of <see cref="Region"/>s</returns>

        public static IEnumerable<Region> GetRegionsCoverage(IEnumerable<Area> areas, int precision)
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
