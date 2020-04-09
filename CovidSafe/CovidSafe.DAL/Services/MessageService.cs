using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.DAL.Helpers;
using CovidSafe.DAL.Repositories;
using CovidSafe.Entities.Protos;

namespace CovidSafe.DAL.Services
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
        public async Task<string> PublishAsync(MatchMessage message, Region region, CancellationToken cancellationToken = default)
        {
            if(message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            // Push to upstream data repository
            return await this._messageRepo.InsertAsync(message, region, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> PublishAreaAsync(AreaMatch areas, CancellationToken cancellationToken = default)
        {
            if (areas == null)
            {
                throw new ArgumentNullException(nameof(areas));
            }

            // Build a MatchMessage containing the submitted areas
            MatchMessage message = new MatchMessage();
            message.AreaMatches.Add(areas);

            // Add to collection to utilize region resolution for potentially many MatchMessages to create
            List<MatchMessage> messages = new List<MatchMessage> { message };

            // Publish
            return await this._messageRepo.InsertManyAsync(messages, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<string> PublishAsync(SelfReportRequest request, long timeAtRequest, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Determine estimated skew
            long estSkew = timeAtRequest - request.ClientTimestamp;

            // Build MatchMessage from submitted content
            MatchMessage message = new MatchMessage();
            BluetoothMatch matches = new BluetoothMatch();

            // Calculate possible offset for each seed
            foreach(BlueToothSeed seed in request.Seeds)
            {
                seed.EstimatedSkew = estSkew;
                matches.Seeds.Add(seed);
            }

            // Store in data repository
            message.BluetoothMatches.Add(matches);
            return await this._messageRepo.InsertAsync(message, request.Region, cancellationToken);
        }
    }
}
