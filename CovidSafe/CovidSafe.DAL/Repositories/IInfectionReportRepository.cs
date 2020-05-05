using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Reports;

namespace CovidSafe.DAL.Repositories
{
    /// <summary>
    /// <see cref="InfectionReport"/> repository definition
    /// </summary>
    public interface IInfectionReportRepository : IRepository<InfectionReport, string>
    {
        /// <summary>
        /// Pulls a list of the latest <see cref="InfectionReport"/> objects, based on client parameters
        /// </summary>
        /// <param name="region">Target <see cref="Region"/></param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="InfectionReport"/> for region, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="InfectionReportMetadata"/> objects</returns>
        Task<IEnumerable<InfectionReportMetadata>> GetLatestAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves the data size, in bytes, of a region's latest <see cref="InfectionReport"/> data
        /// </summary>
        /// <param name="region">Target <see cref="Region"/></param>
        /// <param name="lastTimestamp">Timestamp of latest client <see cref="InfectionReport"/> for region, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Data size, in bytes</returns>
        Task<long> GetLatestRegionSizeAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Pulls a collection of <see cref="InfectionReport"/> objects, based on provided identifiers
        /// </summary>
        /// <param name="ids">Collection of <see cref="InfectionReport"/> identifiers</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="InfectionReport"/> objects</returns>
        Task<IEnumerable<InfectionReport>> GetRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken);
        /// <summary>
        /// Store a new <see cref="InfectionReport"/> in the repository
        /// </summary>
        /// <param name="message"><see cref="InfectionReport"/> to store</param>
        /// <param name="region">Target <see cref="Region"/></param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Unique identifier of stored object</returns>
        Task<string> InsertAsync(InfectionReport message, Region region, CancellationToken cancellationToken = default);
        /// <summary>
        /// Store a new <see cref="InfectionReport"/> in the repository to multiple regions
        /// </summary>
        /// <param name="message"><see cref="InfectionReport"/> to store</param>
        /// <param name="regions">Target <see cref="Region"/></param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task InsertAsync(InfectionReport message, IEnumerable<Region> regions, CancellationToken cancellationToken = default);
    }
}