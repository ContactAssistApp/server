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
        /// <param name="precision">Precision parameter</param>
        /// <remarks>
        /// This constructor allows backward-compatibility with previous 
        /// API versions which defined lat/lng as <see cref="double"/>.
        /// </remarks>
        public static Region CreateRegion(double lat, double lng, int precision = Region.MAX_PRECISION)
        {
            return new Region
            {
                LatitudePrefix = PrecisionHelper.Round(lat, Region.MAX_PRECISION),
                LongitudePrefix = PrecisionHelper.Round(lng, Region.MAX_PRECISION),
                Precision = precision
            };
        }

        /// <summary>
        /// Creates a new <see cref="Region"/>
        /// </summary>
        /// <param name="coordinates">Geographic coordinates object</param>
        /// <param name="precision">Precision parameter</param>
        /// <remarks>
        /// When precise coordinates are provided, it is assumed to be a 
        /// full-precision Region.
        /// </remarks>
        public static Region CreateRegion(Coordinates coordinates, int precision)
        {
            return CreateRegion(coordinates.Latitude, coordinates.Longitude, precision);
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

        private static IEnumerable<Region> GetRegionCoveragePositive(Coordinates location, float radiusMeters, int precision)
        {
            int latMax = (int)Coordinates.MAX_LATITUDE;
            int lonMax = (int)Coordinates.MAX_LONGITUDE;

            int step = PrecisionHelper.GetStep(precision);

            int latCurrent = PrecisionHelper.Round(location.Latitude, precision);
            int latNext = Math.Min(latCurrent + step, latMax);

            int lonCurrent = PrecisionHelper.Round(location.Longitude, precision);
            int lonNext = Math.Min(lonCurrent + step, lonMax);

            //region itself
            yield return new Region(latCurrent, lonCurrent, precision);

            if (latCurrent > 0)
            {
                // South-West
                if (lonCurrent > 0 && GeoHelper.DistanceMeters(location, new Coordinates { Latitude = (float)latCurrent, Longitude = (float)lonCurrent }) < radiusMeters)
                {
                    yield return new Region(latCurrent - step, lonCurrent - step, precision);
                }
                // South 
                if (GeoHelper.DistanceMeters(location, new Coordinates { Latitude = (float)latCurrent, Longitude = location.Longitude }) < radiusMeters)
                {
                    yield return new Region(latCurrent - step, lonCurrent, precision);
                }
                // South-East
                if (GeoHelper.DistanceMeters(location, new Coordinates { Latitude = (float)latCurrent, Longitude = (float)lonNext }) < radiusMeters)
                {
                    yield return new Region(latCurrent - step, lonNext == lonMax ? -lonCurrent : lonNext, precision);
                }
            }
            if (lonCurrent > 0)
            {
                // West
                if (GeoHelper.DistanceMeters(location, new Coordinates { Latitude = location.Latitude, Longitude = (float)lonCurrent }) < radiusMeters)
                {
                    yield return new Region(latCurrent, lonCurrent - step, precision);
                }
                //North-West
                if (latNext < latMax && GeoHelper.DistanceMeters(location, new Coordinates { Latitude = (float)latNext, Longitude = (float)lonCurrent }) < radiusMeters)
                {
                    yield return new Region(latNext, lonCurrent - step, precision);
                }
            }
            if (latNext < latMax)
            {
                //North
                if (GeoHelper.DistanceMeters(location, new Coordinates { Latitude = (float)latNext, Longitude = location.Longitude }) < radiusMeters)
                {
                    yield return new Region(latNext, lonCurrent, precision);
                }
                //North-East
                if (GeoHelper.DistanceMeters(location, new Coordinates { Latitude = (float)latNext, Longitude = (float)lonNext }) < radiusMeters)
                {
                    yield return new Region(latNext, lonNext == lonMax ? -lonCurrent : lonNext, precision);
                }
            }
            //East
            if (GeoHelper.DistanceMeters(location, new Coordinates { Latitude = location.Latitude, Longitude = (float)lonNext }) < radiusMeters)
            {
                if (lonNext < lonMax)
                {
                    yield return new Region(latCurrent, lonNext, precision);
                }
                else if (lonCurrent > 0)
                {
                    yield return new Region(latCurrent, -lonCurrent, precision);
                }
            }
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
            double latAbs = Math.Abs(area.Location.Latitude);
            double lonAbs = Math.Abs(area.Location.Longitude);
            int latSign = Math.Sign(area.Location.Latitude);
            int lonSign = Math.Sign(area.Location.Longitude);

            // Get connected regions from first quadrant 
            var regionsPositive = GetRegionCoveragePositive(new Coordinates { Latitude = latAbs, Longitude = lonAbs }, area.RadiusMeters, precision);

            //returning them back to original quadrants
            foreach (var region in regionsPositive)
            {
                region.LatitudePrefix *= latSign;
                region.LongitudePrefix *= lonSign;
                yield return region; 
            }
        }

        /// <summary>
        /// Enumerates all <see cref="Region"/>s of given precision covering given areas/>
        /// </summary>
        /// <param name="area"><see cref="NarrowcastArea"/>s</param>
        /// <param name="precisionStart">Minimal precision. Any integer number</param>
        /// <param name="precisionEnd">Maximal precision. Any integer number larger than precisionStart</param>
        /// <returns>Collection of <see cref="Region"/>s</returns>

        public static IEnumerable<Region> GetRegionsCoverage(NarrowcastArea area, int precisionStart, int precisionEnd)
        {
            var result = new List<Region>();
            for (int p = precisionStart; p <= precisionEnd; ++p)
            {
                result.AddRange(GetRegionsCoverage(area, p));
            }
            return result;
        }
    }
}