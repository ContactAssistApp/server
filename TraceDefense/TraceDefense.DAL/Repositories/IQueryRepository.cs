using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TraceDefense.Entities;
using TraceDefense.Entities.Geospatial;

namespace TraceDefense.DAL.Repositories
{
    public interface IQueryRepository<QueryId>
    {
        Task<QueryId> Publish(IList<RegionRef> regions, Query query);


        Task<IList<QueryId>> GetQueryIds(IList<RegionRef> regions);

        Task<IList<Query>> GetQueries(IList<QueryId> queryIds);
    }
}
