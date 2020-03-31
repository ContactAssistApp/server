using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.Entities.Interactions;

namespace TraceDefense.DAL.Repositories
{
    /// <summary>
    /// <see cref="Query"/> repository definition
    /// </summary>
    public interface IQueryRepository : IRepository<Query, string>
    {
        /// <summary>
        /// Pulls a list of the latest <see cref="QueryInfo"/>, based on client parameters
        /// </summary>
        /// <param name="regionId">Target region</param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="Query"/> for region, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="QueryInfo"/> objects</returns>
        Task<IList<QueryInfo>> GetLatestAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves the data size, in bytes, of a region's latest <see cref="Query"/> data
        /// </summary>
        /// <param name="regionId">Target region</param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="Query"/> for region, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Data size, in bytes</returns>
        Task<long> GetLatestRegionSizeAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Pulls a collection of <see cref="Query"/> objects by unique identifiers
        /// </summary>
        /// <param name="ids">Collection of <see cref="Query"/> identifiers</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Query"/> objects</returns>
        Task<IList<Query>> GetRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken);
    }
}