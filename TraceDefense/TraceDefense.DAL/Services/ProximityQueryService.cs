using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.DAL.Repositories;
using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Services
{
    /// <summary>
    /// <see cref="ProximityQuery"/> service implementation
    /// </summary>
    public class ProximityQueryService : IProximityQueryService
    {
        /// <summary>
        /// <see cref="ProximityQuery"/> data repository
        /// </summary>
        private IMessageRepository _messageRepo;


        /// <summary>
        /// <see cref="ProximityQuery"/> data repository
        /// </summary>
        private IMessageInfoRepository _messageInfoRepo;

        /// <summary>
        /// Creates a new <see cref="ProximityQueryService"/> instance
        /// </summary>
        /// <param name="queryRepo"><see cref="ProximityQuery"/> data repository</param>
        public ProximityQueryService(IMessageRepository messageRepo, IMessageInfoRepository messageInfoRepo)
        {
            this._messageRepo = messageRepo;
            this._messageInfoRepo = messageInfoRepo;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MatchMessage>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            return await this._messageRepo.GetRangeAsync(ids, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MessageInfo>> GetLatestInfoAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            return await this._messageInfoRepo.GetLatestAsync(region, lastTimestamp, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<long> GetLatestRegionDataSizeAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            return await this._messageInfoRepo.GetLatestRegionSizeAsync(region, lastTimestamp, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task PublishAsync(Region region, MatchMessage message, CancellationToken cancellationToken = default)
        {
            // Push to upstream data repository
            string id = await this._messageRepo.InsertAsync(message, cancellationToken);
            var messageInfo = new MessageInfo { MessageId = id, MessageTimestamp = new UTCTime() };
            await this._messageInfoRepo.UpdateMessageInfoAsync(region, messageInfo, 1, cancellationToken);
        }
    }
}
