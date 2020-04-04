using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.Entities.Protos;

namespace CovidSafe.DAL.Repositories.Mock
{
    /// <summary>
    /// Mock implemnentation of <see cref="IMatchMessageRepository"/>
    /// </summary>
    public class MockMessageRepository : IMatchMessageRepository
    {
        /// <inheritdoc/>
        public Task<MatchMessage> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<MessageInfo>> GetLatestAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<long> GetLatestRegionSizeAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<MatchMessage>> GetRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<string> InsertAsync(Region region, MatchMessage message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
