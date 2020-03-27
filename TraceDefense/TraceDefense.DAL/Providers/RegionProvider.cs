using System;
using System.Collections.Generic;

using TraceDefense.Entities.Geospatial;

namespace TraceDefense.DAL.Providers
{
    /// <summary>
    /// <see cref="RegionRef"/> mapping provider
    /// </summary>
    public static class RegionProvider
    {
        private static float LatStepDegree = 1;
        private static float LonStepDegree = 1;

        /// <summary>
        /// Generates a collection of <see cref="RegionRef"/> objects based on a provided <see cref="Area"/>
        /// </summary>
        /// <param name="area">Target <see cref="Area"/></param>
        /// <returns>Collection of <see cref="RegionRef"/> objects corresponding to provided <see cref="Area"/></returns>
        public static IList<RegionRef> GetRegions(Area area)
        {
            var xmin = (int)(Math.Min(area.First.Latitude, area.Second.Latitude) / LatStepDegree);
            var ymin = (int)(Math.Min(area.First.Longitude, area.Second.Longitude) / LonStepDegree);

            var xmax = (int)(Math.Max(area.First.Latitude, area.Second.Latitude) / LatStepDegree);
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
