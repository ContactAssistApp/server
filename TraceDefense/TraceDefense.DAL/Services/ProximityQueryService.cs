using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.DAL.Repositories;
using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Services
{
    /// <summary>
    /// <see cref="ProximityQuery"/> service implementation
    /// </summary>
    public class ProximityQueryService : IProximityQueryService
    {
        /// <summary>
        /// <see cref="ProximityQuery"/> data repository
        /// </summary>
        private IProximityQueryRepository _queryRepo;

        /// <summary>
        /// Creates a new <see cref="ProximityQueryService"/> instance
        /// </summary>
        /// <param name="queryRepo"><see cref="ProximityQuery"/> data repository</param>
        public ProximityQueryService(IProximityQueryRepository queryRepo)
        {
            this._queryRepo = queryRepo;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProximityQuery>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            return await this._queryRepo.GetRangeAsync(ids, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<QueryInfo>> GetLatestInfoAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            return await this._queryRepo.GetLatestAsync(regionId, lastTimestamp, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<long> GetLatestRegionDataSizeAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            return await this._queryRepo.GetLatestRegionSizeAsync(regionId, lastTimestamp, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<string> PublishAsync(ProximityQuery query, CancellationToken cancellationToken = default)
        {
            // Push to upstream data repository
            return await this._queryRepo.InsertAsync(query, cancellationToken);
        }
    }
}
