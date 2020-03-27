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

        private int TimeStepS = 24 * 3600;

        /// <inheritdoc/>
        public Task<IList<RegionRef>> GetRegions(Area area)
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
            return Task.FromResult(result);
        }
    }
}
