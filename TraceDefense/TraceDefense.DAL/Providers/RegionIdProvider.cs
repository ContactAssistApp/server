using System;
using System.Collections.Generic;
using System.Linq;
using TraceDefense.Entities.Geospatial;
using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Providers
{
    /// <summary>
    /// <see cref="RegionRef"/> mapping provider
    /// </summary>
    /// <remarks>
    /// Used in partition schemes
    /// </remarks>
    public static class RegionIdProvider
    {
        private static float LatStepDegree = 1;
        private static float LonStepDegree = 1;

        /// <summary>
        /// Determines a Region ID based on <see cref="GeoProximity"/>
        /// </summary>
        /// <param name="geo">Source <see cref="GeoProximity"/></param>
        /// <returns>Region identifier</returns>
        /// <remarks>
        /// TODO: Fix this VERY naive implementation.
        /// </remarks>
        public static string FromGeoProximity(IList<GeoProximity> geo)
        {
            // Take first location
            if(geo != null)
            {
                int xMin = Int32.MaxValue;
                int yMin = Int32.MaxValue;
                int xMax = Int32.MinValue;
                int yMax = Int32.MaxValue;

                foreach(GeoProximity prox in geo)
                {
                    int xMinLoc = (int)prox.Locations.Min(l => l.Location.Lattitude);
                    int yMinLoc = (int)prox.Locations.Min(l => l.Location.Longitude);
                    int xMaxLoc = (int)prox.Locations.Max(l => l.Location.Lattitude);
                    int yMaxLoc = (int)prox.Locations.Max(l => l.Location.Longitude);

                    xMin = Math.Min(xMin, xMinLoc);
                    yMin = Math.Min(yMin, yMinLoc);
                    xMax = Math.Max(xMax, xMaxLoc);
                    yMax = Math.Max(yMax, yMaxLoc);
                }

                // Get midpoint (avg)
                int midX = (xMin + xMax) / 2;
                int midY = (yMin + yMax) / 2;

                return String.Format("{0},{1}", midX, midY);
            }
            else
            {
                throw new ArgumentNullException(nameof(geo));
            }
        }

        /// <summary>
        /// Generates a collection of <see cref="RegionRef"/> objects based on a provided <see cref="Area"/>
        /// </summary>
        /// <param name="area">Target <see cref="Area"/></param>
        /// <returns>Collection of <see cref="RegionRef"/> objects corresponding to provided <see cref="Area"/></returns>
        public static IList<RegionRef> GetRegions(Area area)
        {
            var xmin = (int)(Math.Min(area.First.Lattitude, area.Second.Lattitude) / LatStepDegree);
            var ymin = (int)(Math.Min(area.First.Longitude, area.Second.Longitude) / LonStepDegree);

            var xmax = (int)(Math.Max(area.First.Lattitude, area.Second.Lattitude) / LatStepDegree);
            var ymax = (int)(Math.Max(area.First.Longitude, area.Second.Longitude) / LonStepDegree);

            IList<RegionRef> result = new List<RegionRef>();

            for (var x = xmin; x <= xmax; ++x)
            {
                for (var y = ymin; y <= ymax; ++y)
                {
                    result.Add(new RegionRef { Id = $"{x},{y}" });
                }
            }
            return result;
        }
    }
}
