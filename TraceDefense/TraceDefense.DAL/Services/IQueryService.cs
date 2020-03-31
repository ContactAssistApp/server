using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.Entities.Geospatial;
using TraceDefense.Entities.Interactions;

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
        /// <param name="regionId">Unique regiojn identifier</param>
        /// <param name="precision">Precision of region request (number of digits after decimal of coordinate)</param>
        /// <param name="lastTimestamp">Timestamp used to constrain resultset (greater-than), in ms since the UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<IList<Query>> GetByRegionAsync(string regionId, int precision, long lastTimestamp, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the size of the provided <see cref="Query"/> request
        /// </summary>
        /// <param name="regionId">Region to scope</param>
        /// <param name="lastTimestamp">Earliest timestamp allowed (greater than)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<long> GetQueryResultSize(string regionId, long lastTimestamp, CancellationToken cancellationToken = default);

        /// <summary>
        /// Store a new <see cref="Query"/>
        /// </summary>
        /// <param name="query"><see cref="Query"/> to publish</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task PublishAsync(Query query, CancellationToken cancellationToken = default);
    }
}