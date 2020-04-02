using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Repositories
{
    /// <summary>
    /// <see cref="ProximityQuery"/> repository definition
    /// </summary>
    public interface IMessageInfoRepository
    {
        /// <summary>
        /// Pulls a list of the latest message id, based on client parameters
        /// </summary>
        /// <param name="regionId">Target region</param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="MatchMessage"/> for region, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="QueryInfo"/> objects</returns>
        Task<IEnumerable<MessageInfo>> GetLatestAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the data size, in bytes, of a region's latest <see cref="MatchMessage"/> data
        /// </summary>
        /// <param name="regionId">Target region</param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="MatchMessage"/> for region, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Data size, in bytes</returns>
        Task<long> GetLatestRegionSizeAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the data size, in bytes, of a region's latest <see cref="MatchMessage"/> data
        /// </summary>
        /// <param name="regionId">Target region</param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="MatchMessage"/> for region, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Data size, in bytes</returns>
        Task UpdateMessageInfoAsync(Region region, MessageInfo info, long size, CancellationToken cancellationToken = default);

    }
}
