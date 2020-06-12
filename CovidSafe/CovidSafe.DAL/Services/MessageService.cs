using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.DAL.Helpers;
using CovidSafe.DAL.Repositories;
using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Messages;
using CovidSafe.Entities.Validation;

namespace CovidSafe.DAL.Services
{
    /// <summary>
    /// <see cref="MessageService"/> implementation
    /// </summary>
    public class MessageService : IMessageService
    {
        /// <summary>
        /// <see cref="MessageContainer"/> data repository
        /// </summary>
        private IMessageContainerRepository _reportRepo;

        /// <summary>
        /// Precision of regions used for keys.
        /// </summary>
        private int RegionPrecision = 4;

        /// <summary>
        /// Creates a new <see cref="MessageService"/> instance
        /// </summary>
        /// <param name="messageRepo"><see cref="MessageContainer"/> data repository</param>
        public MessageService(IMessageContainerRepository messageRepo)
        {
            this._reportRepo = messageRepo;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MessageContainer>> GetByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            if(ids == null || ids.Count() == 0)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            // Confirm IDs are valid
            RequestValidationResult result = new RequestValidationResult();

            foreach(string messageId in ids)
            {
                result.Combine(Validator.ValidateGuid(messageId, nameof(messageId)));
            }

            // Verify results were validated
            if(result.Passed)
            {
                return await this._reportRepo.GetRangeAsync(ids, cancellationToken);
            }
            else
            {
                throw new RequestValidationFailedException(result);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MessageContainerMetadata>> GetLatestInfoAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }
            else
            {
                // Validate region
                RequestValidationResult validationResult = region.Validate();

                // Validate timestamp
                validationResult.Combine(Validator.ValidateTimestamp(lastTimestamp));

                if(validationResult.Passed)
                {
                    // Get message information from database
                    return await this._reportRepo.GetLatestAsync(region, lastTimestamp, cancellationToken);
                }
                else
                {
                    throw new RequestValidationFailedException(validationResult);
                }
            }
        }

        /// <inheritdoc/>
        public async Task<long> GetLatestRegionDataSizeAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            if(region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            RequestValidationResult validationResult = region.Validate();
            validationResult.Combine(Validator.ValidateTimestamp(lastTimestamp, parameterName: nameof(lastTimestamp)));

            if(validationResult.Passed)
            {
                // Get messages from database
                return await this._reportRepo.GetLatestRegionSizeAsync(region, lastTimestamp, cancellationToken);
            }
            else
            {
                throw new RequestValidationFailedException(validationResult);
            }
        }

        /// <inheritdoc/>
        public async Task PublishAreaAsync(NarrowcastMessage message, CancellationToken cancellationToken = default)
        {
            // Validate inputs
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            RequestValidationResult validationResult = message.Validate();

            if(validationResult.Passed)
            {
                // Build a MessageContainer for the submitted NarrowcastMessage
                MessageContainer container = new MessageContainer();
                container.Narrowcasts.Add(message);

                // Define regions for the published message
                IEnumerable<Region> messageRegions = RegionHelper.GetRegionsCoverage(message.Areas, this.RegionPrecision);

                // Publish
                await this._reportRepo.InsertAsync(container, messageRegions, cancellationToken);
            }
            else
            {
                throw new RequestValidationFailedException(validationResult);
            }
        }

        /// <inheritdoc/>
        public async Task<string> PublishAsync(MessageContainer report, Region region, CancellationToken cancellationToken = default)
        {
            // Validate inputs
            if(report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            RequestValidationResult validationResult = report.Validate();
            validationResult.Combine(region.Validate());

            if(validationResult.Passed)
            {
                // Push to upstream data repository
                return await this._reportRepo.InsertAsync(report, region, cancellationToken);
            }
            else
            {
                throw new RequestValidationFailedException(validationResult);
            }
        }

        /// <inheritdoc/>
        public async Task<string> PublishAsync(IEnumerable<BluetoothSeedMessage> seeds, Region region, long timeAtRequest, CancellationToken cancellationToken = default)
        {
            // Validate inputs
            if (seeds == null || seeds.Count() == 0)
            {
                throw new ArgumentNullException(nameof(seeds));
            }
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            // Validate timestamp
            RequestValidationResult validationResult = Validator.ValidateTimestamp(timeAtRequest);

            // Validate seeds
            foreach(BluetoothSeedMessage seed in seeds)
            {
                validationResult.Combine(seed.Validate());
            }

            if(validationResult.Passed)
            {
                // Build MatchMessage from submitted content
                MessageContainer report = new MessageContainer();

                if(report.BluetoothSeeds is List<BluetoothSeedMessage> asList)
                {
                    asList.AddRange(seeds);
                }
                else
                {
                    foreach(BluetoothSeedMessage seed in seeds)
                    {
                        report.BluetoothSeeds.Add(seed);
                    }
                }

                // Store in data repository
                return await this._reportRepo.InsertAsync(report, region, cancellationToken);
            }
            else
            {
                throw new RequestValidationFailedException(validationResult);
            }
        }
    }
}
