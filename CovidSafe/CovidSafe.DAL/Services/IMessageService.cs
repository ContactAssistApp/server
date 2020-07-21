using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Messages;

namespace CovidSafe.DAL.Services
{
    /// <summary>
    /// <see cref="MessageContainer"/> service layer definition
    /// </summary>
    public interface IMessageService : IService
    {
        /// <summary>
        /// Deletes a <see cref="MessageContainer"/> based on its unique identifier
        /// </summary>
        /// <param name="id">Identifier of <see cref="MessageContainer"/> to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteMessageByIdAsync(string id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves a collection of <see cref="MessageContainer"/> objects by their unique identifiers
        /// </summary>
        /// <param name="ids">Collection of <see cref="MessageContainer"/> identifiers</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="MessageContainer"/> objects</returns>
        Task<IEnumerable<MessageContainer>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves the latest <see cref="MessageContainerMetadata"/> for a client
        /// </summary>
        /// <param name="region">Target <see cref="Region"/></param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="MessageContainer"/>, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="MessageContainerMetadata"/> objects</returns>
        /// <remarks>
        /// Sister call to <see cref="GetLatestRegionDataSizeAsync(Region, long, CancellationToken)"/>.
        /// </remarks>
        Task<IEnumerable<MessageContainerMetadata>> GetLatestInfoAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns the latest size of <see cref="MessageContainer"/> data for a given <see cref="Region"/>
        /// </summary>
        /// <param name="region">Target <see cref="Region"/></param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="MessageContainer"/>, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<long> GetLatestRegionDataSizeAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Store a new <see cref="MessageContainer"/> based on a <see cref="NarrowcastMessage"/>
        /// </summary>
        /// <param name="areaMatch"><see cref="NarrowcastMessage"/> content</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of published <see cref="MessageContainer"/> identifiers</returns>
        Task PublishAreaAsync(NarrowcastMessage areaMatch, CancellationToken cancellationToken = default);
        /// <summary>
        /// Store a new collection of <see cref="BluetoothSeedMessage"/> objects
        /// </summary>
        /// <param name="seeds">Collection of <see cref="BluetoothSeedMessage"/> objects to store</param>
        /// <param name="region">Target <see cref="Region"/> of seeds</param>
        /// <param name="serverTimestamp">Server timestamp when request was received, in ms since UNIX epoch (for calculating skew)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Published <see cref="MessageContainer"/> identifier</returns>
        Task<string> PublishAsync(IEnumerable<BluetoothSeedMessage> seeds, Region region, long serverTimestamp, CancellationToken cancellationToken = default);
    }
}