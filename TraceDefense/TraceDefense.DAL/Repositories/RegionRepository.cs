using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TraceDefense.Entities.Geospatial;
using TraceDefense.Entities.Search;

namespace TraceDefense.DAL.Repositories
{
    /// <summary>
    /// In-memory <see cref="RegionRef"/> repository implementation
    /// </summary>
    public class RegionRepository : IRegionRepository
    {
        private float LatStepDegree = 1;

        private float LonStepDegree = 1;

        private int TimeStepMs = 24 * 60 * 1000;

        public async Task<IList<RegionRef>> GetRegions(Area area)
        {
            var xmin = (int)(Math.Min(area.First.Latitude, area.Second.Latitude) / LatStepDegree);
            var ymin = (int)(Math.Min(area.First.Longitude, area.Second.Longitude) / LonStepDegree);
            var tmin = (int)(area.TimeRange.StartTimeMs / TimeStepMs);

            var xmax = (int)(Math.Max(area.First.Latitude, area.Second.Latitude) / LatStepDegree);
            var ymax = (int)(Math.Max(area.First.Longitude, area.Second.Longitude) / LonStepDegree);
            var tmax = (int)(area.TimeRange.StartTimeMs / TimeStepMs);

            var result = new List<RegionRef>();

            for (var x = xmin; xmin <= xmax; ++x)
            {
                for (var y = ymin; ymin <= ymax; ++y)
                {
                    for (var t = tmin; t <= tmax; ++t)
                    {
                        result.Add(new RegionRef { X = x, Y = y, T = t });
                    }
                }
            }
            return result;
        }
    }
}
