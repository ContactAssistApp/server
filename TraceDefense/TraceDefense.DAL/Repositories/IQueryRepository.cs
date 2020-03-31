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
    public interface IQueryRepository : IRepository<Query>
    {
        /// <summary>
        /// Pulls a collection of query data based on region identifier from the timestamp provided
        /// </summary>
        /// <param name="regionId">Unique <see cref="RegionRef"/> identifier</param>
        /// <param name="lastTimestamp">Timestamp used for 'greater-than' filter against <see cref="Query"/> results</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Query"/> data</returns>
        Task<IList<Query>> GetQueriesAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default);
        /// <summary>
        /// Stores a new <see cref="Query"/>
        /// </summary>
        /// <param name="regions">Region applicable to posted <see cref="Query"/></param>
        /// <param name="query"><see cref="Query"/> content</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>New <see cref="Query"/> identifier</returns>
        Task PublishAsync(IList<RegionRef> regions, Query query, CancellationToken cancellationToken = default);
    }
}