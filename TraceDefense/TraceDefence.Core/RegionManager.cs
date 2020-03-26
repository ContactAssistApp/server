using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraceDefense.DAL.Repositories;
using TraceDefense.Entities.Geospatial;
using TraceDefense.Entities.Search;

namespace TraceDefence.Core
{
    public class RegionManager : IRegionManager
    {
        public Task<IList<RegionRef>> GetRegions(Area area)
        {
            throw new NotImplementedException();
        }
    }
}
