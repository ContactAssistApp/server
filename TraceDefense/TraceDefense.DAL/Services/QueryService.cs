using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.DAL.Repositories;
using TraceDefense.Entities.Geospatial;
using TraceDefense.Entities.Interactions;

namespace TraceDefense.DAL.Services
{
    /// <summary>
    /// <see cref="Query"/> service implementation
    /// </summary>
    public class QueryService : IQueryService
    {
        /// <summary>
        /// <see cref="Query"/> data repository
        /// </summary>
        private IQueryRepository _queryRepo;

        /// <summary>
        /// Creates a new <see cref="QueryService"/> instance
        /// </summary>
        /// <param name="queryRepo"><see cref="Query"/> data repository</param>
        public QueryService(IQueryRepository queryRepo)
        {
            this._queryRepo = queryRepo;
        }

        /// <inheritdoc/>
        public async Task<IList<Query>> GetByRegionAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            return await this._queryRepo.GetQueriesAsync(regionId, lastTimestamp, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task PublishAsync(IList<RegionRef> regions, Query query, CancellationToken cancellationToken = default)
        {
            await this._queryRepo.PublishAsync(regions, query, cancellationToken);
        }
    }
}
