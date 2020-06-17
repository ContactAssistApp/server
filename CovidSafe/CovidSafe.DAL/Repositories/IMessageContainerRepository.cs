using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Messages;

namespace CovidSafe.DAL.Repositories
{
    /// <summary>
    /// <see cref="MessageContainer"/> repository definition
    /// </summary>
    public interface IMessageContainerRepository : IRepository<MessageContainer, string>
    {
        /// <summary>
        /// Pulls a list of the latest <see cref="MessageContainer"/> objects, based on client parameters
        /// </summary>
        /// <param name="region">Target <see cref="Region"/></param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="MessageContainer"/> for region, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="MessageContainerMetadata"/> objects</returns>
        Task<IEnumerable<MessageContainerMetadata>> GetLatestAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves the data size, in bytes, based on a region's latest <see cref="MessageContainer"/> data
        /// </summary>
        /// <param name="region">Target <see cref="Region"/></param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="MessageContainer"/> for region, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Data size, in bytes</returns>
        Task<long> GetLatestRegionSizeAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Pulls a collection of <see cref="MessageContainer"/> objects, based on provided identifiers
        /// </summary>
        /// <param name="ids">Collection of <see cref="MessageContainer"/> identifiers</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="MessageContainer"/> objects</returns>
        Task<IEnumerable<MessageContainer>> GetRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken);
        /// <summary>
        /// Store a new <see cref="MessageContainer"/> in the repository
        /// </summary>
        /// <param name="message"><see cref="MessageContainer"/> to store</param>
        /// <param name="region">Target <see cref="Region"/></param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Unique identifier of stored object</returns>
        Task<string> InsertAsync(MessageContainer message, Region region, CancellationToken cancellationToken = default);
        /// <summary>
        /// Store a new <see cref="MessageContainer"/> in the repository to multiple regions
        /// </summary>
        /// <param name="message"><see cref="MessageContainer"/> to store</param>
        /// <param name="regions">Target <see cref="Region"/></param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task InsertAsync(MessageContainer message, IEnumerable<Region> regions, CancellationToken cancellationToken = default);
    }
}