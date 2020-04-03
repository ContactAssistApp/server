using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TraceDefense.DAL.Providers;
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
        private IMessageRepository _messageRepo;

        /// <summary>
        /// Creates a new <see cref="MessageService"/> instance
        /// </summary>
        /// <param name="messageRepo"><see cref="MatchMessage"/> data repository</param>
        public MessageService(IMessageRepository messageRepo)
        {
            this._messageRepo = messageRepo;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MatchMessage>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            return await this._messageRepo.GetRangeAsync(ids, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MessageInfo>> GetLatestInfoAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            var range = RegionHelper.ToRange(region);
            return await this._messageRepo.GetLatestAsync(range.Item1, range.Item2, lastTimestamp, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<long> GetLatestRegionDataSizeAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            var range = RegionHelper.ToRange(region);
            return await this._messageRepo.GetLatestRegionSizeAsync(range.Item1, range.Item2, lastTimestamp, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task PublishAsync(Region region, MatchMessage message, CancellationToken cancellationToken = default)
        {
            var range = RegionHelper.ToRange(region);
            // Push to upstream data repository
            await this._messageRepo.InsertAsync(range.Item1, range.Item2, message, cancellationToken);
        }
    }
}
