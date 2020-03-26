using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TraceDefense.Entities.Geospatial;
using TraceDefense.Entities.Search;

namespace TraceDefense.DAL.Repositories
{
    public interface IRegionManager
    {
        Task<IList<RegionRef>> GetRegions(Area area);
    }
}
