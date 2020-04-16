using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.Entities.Protos;

namespace CovidSafe.DAL.Services
{
    /// <summary>
    /// <see cref="MatchMessage"/> service layer definition
    /// </summary>
    public interface IMessageService : IService
    {
        /// <summary>
        /// Retrieves a collection of <see cref="MatchMessage"/> objects by their unique identifiers
        /// </summary>
        /// <param name="ids">Collection of <see cref="MatchMessage"/> identifiers</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="MatchMessage"/> objects</returns>
        Task<IEnumerable<MatchMessage>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves the latest <see cref="MessageInfo"/> for a client
        /// </summary>
        /// <param name="region">Target <see cref="Region"/></param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="MatchMessage"/>, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="MessageInfo"/> objects</returns>
        /// <remarks>
        /// Sister call to <see cref="GetLatestRegionDataSizeAsync(Region, long, CancellationToken)"/>.
        /// </remarks>
        Task<IEnumerable<MessageInfo>> GetLatestInfoAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns the latest size of <see cref="MatchMessage"/> data for a given <see cref="Region"/>
        /// </summary>
        /// <param name="region">Target <see cref="Region"/></param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="MatchMessage"/>, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<long> GetLatestRegionDataSizeAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Store a new <see cref="MatchMessage"/> based on an <see cref="AreaMatch"/>
        /// </summary>
        /// <param name="areaMatch"><see cref="AreaMatch"/> content</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of published <see cref="MatchMessage"/> identifiers</returns>
        Task PublishAreaAsync(AreaMatch areaMatch, CancellationToken cancellationToken = default);
        /// <summary>
        /// Store a new <see cref="SelfReportRequest"/>
        /// </summary>
        /// <param name="report">Source <see cref="SelfReportRequest"/></param>
        /// <param name="serverTimestamp">Server timestamp when request was received, in ms since UNIX epoch (for calculating skew)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Published <see cref="MatchMessage"/> identifier</returns>
        Task<string> PublishAsync(SelfReportRequest report, long serverTimestamp, CancellationToken cancellationToken = default);
    }
}