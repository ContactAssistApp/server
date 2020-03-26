using System.Collections.Generic;
using System.Threading.Tasks;

using TraceDefense.Entities.Geospatial;
using TraceDefense.Entities.Search;

namespace TraceDefense.DAL.Repositories
{
    /// <summary>
    /// <see cref="RegionRef"/> repository definition
    /// </summary>
    public interface IRegionRepository : IRepository<RegionRef>
    {
        Task<IList<RegionRef>> GetRegions(Area area);
    }
}
