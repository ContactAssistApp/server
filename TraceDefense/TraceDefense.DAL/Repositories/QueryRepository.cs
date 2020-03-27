using System;
using System.Collections.Generic;
using System.Threading;
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
        public Task<IList<Query>> GetQueriesAsync(IList<string> queryIds, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetQueryIdsAsync(IList<RegionRef> regions, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> PublishAsync(Query query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
