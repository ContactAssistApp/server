using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Reports;

namespace CovidSafe.DAL.Services
{
    /// <summary>
    /// <see cref="InfectionReport"/> service layer definition
    /// </summary>
    public interface IInfectionReportService : IService
    {
        /// <summary>
        /// Retrieves a collection of <see cref="InfectionReport"/> objects by their unique identifiers
        /// </summary>
        /// <param name="ids">Collection of <see cref="InfectionReport"/> identifiers</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="InfectionReport"/> objects</returns>
        Task<IEnumerable<InfectionReport>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves the latest <see cref="InfectionReportMetadata"/> for a client
        /// </summary>
        /// <param name="region">Target <see cref="Region"/></param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="InfectionReport"/>, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="InfectionReportMetadata"/> objects</returns>
        /// <remarks>
        /// Sister call to <see cref="GetLatestRegionDataSizeAsync(Region, long, CancellationToken)"/>.
        /// </remarks>
        Task<IEnumerable<InfectionReportMetadata>> GetLatestInfoAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns the latest size of <see cref="InfectionReport"/> data for a given <see cref="Region"/>
        /// </summary>
        /// <param name="region">Target <see cref="Region"/></param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="InfectionReport"/>, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<long> GetLatestRegionDataSizeAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Store a new <see cref="InfectionReport"/> based on an <see cref="AreaReport"/>
        /// </summary>
        /// <param name="areaMatch"><see cref="AreaReport"/> content</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of published <see cref="InfectionReport"/> identifiers</returns>
        Task PublishAreaAsync(AreaReport areaMatch, CancellationToken cancellationToken = default);
        /// <summary>
        /// Store a new collection of <see cref="BluetoothSeed"/> objects
        /// </summary>
        /// <param name="seeds">Collection of <see cref="BluetoothSeed"/> objects to store</param>
        /// <param name="region">Target <see cref="Region"/> of seeds</param>
        /// <param name="serverTimestamp">Server timestamp when request was received, in ms since UNIX epoch (for calculating skew)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Published <see cref="InfectionReport"/> identifier</returns>
        Task<string> PublishAsync(IEnumerable<BluetoothSeed> seeds, Region region, long serverTimestamp, CancellationToken cancellationToken = default);
    }
}