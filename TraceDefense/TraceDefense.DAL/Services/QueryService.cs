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
        public async Task<IList<Query>> GetByRegionAsync(string regionId, int precision, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            return await this._queryRepo.GetByRegionAsync(regionId, precision, lastTimestamp, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<long> GetQueryResultSize(string regionId, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            // TODO: Implement properly
            return Task.FromResult((long) 100);
        }

        /// <inheritdoc/>
        public async Task PublishAsync(Query query, CancellationToken cancellationToken = default)
        {
            await this._queryRepo.InsertAsync(query, cancellationToken);
        }
    }
}
