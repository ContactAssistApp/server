using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.DAL.Repositories;
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
        public async Task<IList<Query>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            return await this._queryRepo.GetRangeAsync(ids, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IList<QueryInfo>> GetLatestInfoAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            return await this._queryRepo.GetLatestAsync(regionId, lastTimestamp, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<long> GetLatestRegionSizeAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            return await this._queryRepo.GetLatestRegionSizeAsync(regionId, lastTimestamp, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<string> PublishAsync(Query query, CancellationToken cancellationToken = default)
        {
            // Push to upstream data repository
            return await this._queryRepo.InsertAsync(query, cancellationToken);
        }
    }
}
