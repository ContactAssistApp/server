using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TraceDefense.DAL.Repositories;
using TraceDefense.Entities;
using TraceDefense.Entities.Geospatial;

namespace TraceDefence.Core
{
    public class QueryRepository : IQueryRepository<int>
    {
        public Task<IList<Query>> GetQueries(IList<int> queryIds)
        {
            throw new NotImplementedException();
        }

        public Task<IList<int>> GetQueryIds(IList<RegionRef> regions)
        {
            throw new NotImplementedException();
        }

        public Task<int> Publish(IList<RegionRef> regions, Query query)
        {
            throw new NotImplementedException();
        }
    }
}
