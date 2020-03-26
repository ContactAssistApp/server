using System.Collections.Generic;
using System.Threading.Tasks;

using TraceDefense.Entities;
using TraceDefense.Entities.Geospatial;

namespace TraceDefense.DAL.Repositories
{
    /// <summary>
    /// <see cref="Query"/> repository definition
    /// </summary>
    public interface IQueryRepository : IRepository<Query>
    {
        Task<Query> Publish(IList<RegionRef> regions, Query query);


        Task<IList<Query>> GetQueryIds(IList<RegionRef> regions);

        Task<IList<Query>> GetQueries(IList<int> queryIds);
    }
}
