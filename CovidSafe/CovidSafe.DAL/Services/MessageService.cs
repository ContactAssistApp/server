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

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> PublishAreaAsync(AreaMatch areas, CancellationToken cancellationToken = default)
        {
            if (areas == null)
            {
                throw new ArgumentNullException(nameof(areas));
            }

            // Determine regions for each area
            Dictionary<string, AreaMatch> areaMatches = new Dictionary<string, AreaMatch>();

            foreach(Area area in areas.Areas)
            {
                // Get RegionID of area
                string regionId = RegionHelper.GetRegionIdentifier(area);

                if(areaMatches.ContainsKey(regionId))
                {
                    // Add area match
                    areaMatches[regionId].Areas.Add(area);
                }
                else
                {
                    AreaMatch match = new AreaMatch();
                    match.UserMessage = areas.UserMessage;
                    match.Areas.Add(area);

                    areaMatches[regionId] = match;
                }
            }

            // Build MatchMessage from submitted content
            // TODO: Execute in a transaction
            List<string> createdIdentifiers = new List<string>();
            foreach(KeyValuePair<string, AreaMatch> match in areaMatches)
            {
                MatchMessage message = new MatchMessage();
                message.AreaMatches.Add(match.Value);

                string id =await this._messageRepo.InsertAsync(
                    RegionHelper.FromRegionIdentifier(match.Key),
                    message,
                    cancellationToken
                );
                createdIdentifiers.Add(id);
            }

            return createdIdentifiers;
        }

        /// <inheritdoc/>
        public async Task<string> PublishAsync(IEnumerable<BlueToothSeed> seeds, Region region, CancellationToken cancellationToken = default)
        {
            if (seeds == null || seeds.Count() == 0)
            {
                throw new ArgumentNullException(nameof(seeds));
            }
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            // Build MatchMessage from submitted content
            MatchMessage message = new MatchMessage();
            BluetoothMatch matches = new BluetoothMatch();
            matches.Seeds.AddRange(seeds);
            message.BluetoothMatches.Add(matches);

            // Store in data repository
            return await this._messageRepo.InsertAsync(region, message, cancellationToken);
        }
    }
}
