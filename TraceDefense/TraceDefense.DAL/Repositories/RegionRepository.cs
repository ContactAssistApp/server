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
        public Task<IList<RegionRef>> GetRegions(Area area)
        {
            throw new NotImplementedException();
        }
    }
}
