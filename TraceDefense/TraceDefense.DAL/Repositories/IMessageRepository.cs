using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Repositories
{
    /// <summary>
    /// <see cref="ProximityQuery"/> repository definition
    /// </summary>
    public interface IMessageRepository : IRepository<MatchMessage, string>
    {
        /// <summary>
        /// Pulls a collection of <see cref="MatchMessage"/> objects by unique identifiers
        /// </summary>
        /// <param name="ids">Collection of <see cref="ProximityQuery"/> identifiers</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="ProximityQuery"/> objects</returns>
        Task<IEnumerable<MatchMessage>> GetRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken);
    }
}