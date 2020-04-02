using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Repositories
{
    /// <summary>
    /// <see cref="ProximityQuery"/> repository definition
    /// </summary>
    public interface IMessageRepository
    {
        /// <summary>
        /// Retrieves a specific object from the repository
        /// </summary>
        /// <param name="id">Unique object identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Matching object or null</returns>
        Task<MatchMessage> GetAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Store a new object in the repository
        /// </summary>
        /// <param name="region">Coarse region associated with the message</param>
        /// <param name="message"<see cref="cref="MatchMessage"/>>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Unique identifier of stored object</returns>
        Task InsertAsync(Location locmin, Location locmax, MatchMessage message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Pulls a list of the latest <see cref="QueryInfo"/>, based on client parameters
        /// </summary>
        /// <param name="regionId">Target region</param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="ProximityQuery"/> for region, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="QueryInfo"/> objects</returns>
        Task<IEnumerable<MessageInfo>> GetLatestAsync(Location locmin, Location locmax, long lastTimestamp, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the data size, in bytes, of a region's latest <see cref="ProximityQuery"/> data
        /// </summary>
        /// <param name="regionId">Target region</param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="ProximityQuery"/> for region, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Data size, in bytes</returns>
        Task<long> GetLatestRegionSizeAsync(Location locmin, Location locmax, long lastTimestamp, CancellationToken cancellationToken = default);

        /// <summary>
        /// Pulls a collection of <see cref="MatchMessage"/> objects by unique identifiers
        /// </summary>
        /// <param name="ids">Collection of <see cref="ProximityQuery"/> identifiers</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="ProximityQuery"/> objects</returns>
        Task<IEnumerable<MatchMessage>> GetRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken);
    }
}