using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.Entities.Geospatial;
using TraceDefense.Entities.Interactions;

namespace TraceDefense.DAL.Repositories
{
    /// <summary>
    /// <see cref="Query"/> repository definition
    /// </summary>
    public interface IQueryRepository : IRepository<Query, string>
    {
        /// <summary>
        /// Pulls a collection of query data based on region, precision, and filtered by timestamp
        /// </summary>
        /// <param name="regionId">Unique <see cref="RegionRef"/> identifier</param>
        /// <param name="precision">Precision used to scope matching <see cref="Query"/> objects</param>
        /// <param name="lastTimestamp">Timestamp used for 'greater-than' filter against <see cref="Query"/> results</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Query"/> data</returns>
        Task<IList<Query>> GetByRegionAsync(string regionId, int precision, long lastTimestamp, CancellationToken cancellationToken = default);
    }
}