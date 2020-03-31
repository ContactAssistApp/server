using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.Entities.Geospatial;
using TraceDefense.Entities.Schemas;

namespace TraceDefense.DAL.Services
{
    /// <summary>
    /// Base <see cref="Query"/> service definition
    /// </summary>
    public interface IQueryService : IService
    {
        /// <summary>
        /// Retrieve a collection of queries based on <see cref="RegionRef"/> identifier, after the provided timestamp (ms since UNIX epoch)
        /// </summary>
        /// <param name="regionId">Unique <see cref="RegionRef"/> identifier</param>
        /// <param name="lastTimestamp">Timestamp used to constrain resultset (greater-than), in ms since the UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<IList<Query>> GetByRegionAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Store a new <see cref="Query"/>
        /// </summary>
        /// <param name="regions">Regions applicable to published <see cref="Query"/></param>
        /// <param name="query"><see cref="Query"/> to publish</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task PublishAsync(IList<RegionRef> regions, Query query, CancellationToken cancellationToken = default);
    }
}
