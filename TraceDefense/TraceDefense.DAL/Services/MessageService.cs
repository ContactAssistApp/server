using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.DAL.Repositories;
using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Services
{
    /// <summary>
    /// <see cref="MatchMessage"/> service implementation
    /// </summary>
    public class MessageService : IMessageService
    {
        /// <summary>
        /// <see cref="MatchMessage"/> data repository
        /// </summary>
        private IMatchMessageRepository _messageRepo;

        /// <summary>
        /// Creates a new <see cref="MessageService"/> instance
        /// </summary>
        /// <param name="messageRepo"><see cref="MatchMessage"/> data repository</param>
        public MessageService(IMatchMessageRepository messageRepo)
        {
            this._messageRepo = messageRepo;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MatchMessage>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            if(ids == null || ids.Count() == 0)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            // Pass-through call, no additional processing required
            return await this._messageRepo.GetRangeAsync(ids, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MessageInfo>> GetLatestInfoAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }
            if (lastTimestamp < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lastTimestamp));
            }

            // Get message information from database
            return await this._messageRepo.GetLatestAsync(region, lastTimestamp, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<long> GetLatestRegionDataSizeAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            if(region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }
            if(lastTimestamp < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lastTimestamp));
            }

            // Get messages from database
            return await this._messageRepo.GetLatestRegionSizeAsync(region, lastTimestamp, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<string> PublishAsync(Region region, MatchMessage message, CancellationToken cancellationToken = default)
        {
            if(region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }
            if(message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            // Push to upstream data repository
            return await this._messageRepo.InsertAsync(region, message, cancellationToken);
        }
    }
}
