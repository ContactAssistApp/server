using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Repositories
{
    /// <summary>
    /// <see cref="ProximityQuery"/> repository definition
    /// </summary>
    public interface IProximityQueryRepository : IRepository<ProximityQuery, string>
    {
        /// <summary>
        /// Pulls a list of the latest <see cref="QueryInfo"/>, based on client parameters
        /// </summary>
        /// <param name="regionId">Target region</param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="ProximityQuery"/> for region, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="QueryInfo"/> objects</returns>
        Task<IEnumerable<QueryInfo>> GetLatestAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves the data size, in bytes, of a region's latest <see cref="ProximityQuery"/> data
        /// </summary>
        /// <param name="regionId">Target region</param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="ProximityQuery"/> for region, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Data size, in bytes</returns>
        Task<long> GetLatestRegionSizeAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Pulls a collection of <see cref="ProximityQuery"/> objects by unique identifiers
        /// </summary>
        /// <param name="ids">Collection of <see cref="ProximityQuery"/> identifiers</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="ProximityQuery"/> objects</returns>
        Task<IEnumerable<ProximityQuery>> GetRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken);
    }
}