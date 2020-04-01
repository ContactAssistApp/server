using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Services
{
    /// <summary>
    /// <see cref="ProximityQuery"/> service layer definition
    /// </summary>
    public interface IProximityQueryService : IService
    {
        /// <summary>
        /// Retrieves a collection of <see cref="ProximityQuery"/> objects by their unique identifiers
        /// </summary>
        /// <param name="ids">Collection of <see cref="ProximityQuery"/> identifiers</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="ProximityQuery"/> objects</returns>
        Task<IEnumerable<ProximityQuery>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves the latest <see cref="QueryInfo"/> for a client
        /// </summary>
        /// <param name="regionId">Target region identifier</param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="ProximityQuery"/>, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="QueryInfo"/> objects</returns>
        /// <remarks>
        /// Sister call to <see cref="GetLatestRegionDataSizeAsync(string, long, CancellationToken)"/>.
        /// </remarks>
        Task<IEnumerable<QueryInfo>> GetLatestInfoAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns the latest size of <see cref="ProximityQuery"/> data for a given region
        /// </summary>
        /// <param name="regionId">Region identifier</param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="ProximityQuery"/>, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<long> GetLatestRegionDataSizeAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Store a new <see cref="ProximityQuery"/>
        /// </summary>
        /// <param name="query"><see cref="ProximityQuery"/> to publish</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Published <see cref="ProximityQuery"/> identifier</returns>
        Task<string> PublishAsync(ProximityQuery query, CancellationToken cancellationToken = default);
    }
}