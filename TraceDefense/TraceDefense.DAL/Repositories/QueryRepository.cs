using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TraceDefense.Entities;
using TraceDefense.Entities.Geospatial;

namespace TraceDefense.DAL.Repositories
{
    /// <summary>
    /// In-memory <see cref="Query"/> repository implementation
    /// </summary>
    public class QueryRepository : IQueryRepository
    {
        public Task<IList<Query>> GetQueries(IList<int> queryIds)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Query>> GetQueryIds(IList<RegionRef> regions)
        {
            throw new NotImplementedException();
        }

        public Task<Query> Publish(IList<RegionRef> regions, Query query)
        {
            throw new NotImplementedException();
        }
    }
}
