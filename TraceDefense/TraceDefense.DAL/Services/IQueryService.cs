using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.Entities.Interactions;

namespace TraceDefense.DAL.Services
{
    /// <summary>
    /// <see cref="Query"/> service layer definition
    /// </summary>
    public interface IQueryService : IService
    {
        /// <summary>
        /// Retrieves a collection of <see cref="Query"/> objects by their unique identifiers
        /// </summary>
        /// <param name="ids">Collection of <see cref="Query"/> identifiers</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Query"/> objects</returns>
        Task<IList<Query>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves the latest <see cref="QueryInfo"/> for a client
        /// </summary>
        /// <param name="regionId">Target region identifier</param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="Query"/>, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="QueryInfo"/> objects</returns>
        /// <remarks>
        /// Sister call to <see cref="IQueryService.GetLatestRegionSizeAsync(string, long, CancellationToken)"/>.
        /// </remarks>
        Task<IList<QueryInfo>> GetLatestInfoAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns the latest size of <see cref="Query"/> data for a given region
        /// </summary>
        /// <param name="regionId">Region identifier</param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="Query"/>, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<long> GetLatestRegionSizeAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Store a new <see cref="Query"/>
        /// </summary>
        /// <param name="query"><see cref="Query"/> to publish</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Published <see cref="Query"/> identifier</returns>
        Task<string> PublishAsync(Query query, CancellationToken cancellationToken = default);
    }
}