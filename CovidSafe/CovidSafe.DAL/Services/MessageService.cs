using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.DAL.Helpers;
using CovidSafe.DAL.Repositories;
using CovidSafe.Entities.Protos;
using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;

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
        /// Precision of regions that are used for keys.
        /// </summary>
        private int RegionPrecision = 4;

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

            // Confirm IDs are valid
            RequestValidationResult result = new RequestValidationResult();

            foreach(string messageId in ids)
            {
                result.Combine(Validator.ValidateGuid(messageId, nameof(messageId)));
            }

            // Verify results were validated
            if(result.Passed)
            {
                return await this._messageRepo.GetRangeAsync(ids, cancellationToken);
            }
            else
            {
                throw new RequestValidationFailedException(result);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MessageInfo>> GetLatestInfoAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default)
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
                    return await this._messageRepo.GetLatestAsync(region, lastTimestamp, cancellationToken);
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
                return await this._messageRepo.GetLatestRegionSizeAsync(region, lastTimestamp, cancellationToken);
            }
            else
            {
                throw new RequestValidationFailedException(validationResult);
            }
        }

        /// <inheritdoc/>
        public async Task PublishAreaAsync(AreaMatch areaMatch, CancellationToken cancellationToken = default)
        {
            // Validate inputs
            if (areaMatch == null)
            {
                throw new ArgumentNullException(nameof(areaMatch));
            }

            RequestValidationResult validationResult = areaMatch.Validate();

            if(validationResult.Passed)
            {
                // Build a MatchMessage containing the submitted areas
                MatchMessage message = new MatchMessage();
                message.AreaMatches.Add(areaMatch);

                // Define a regions for the published message
                IEnumerable<Region> messageRegions = RegionHelper.GetRegionsCoverage(areaMatch.Areas, this.RegionPrecision);

                // Publish
                await this._messageRepo.InsertAsync(message, messageRegions, cancellationToken);
            }
            else
            {
                throw new RequestValidationFailedException(validationResult);
            }
        }

        /// <inheritdoc/>
        public async Task<string> PublishAsync(MatchMessage message, Region region, CancellationToken cancellationToken = default)
        {
            // Validate inputs
            if(message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            RequestValidationResult validationResult = message.Validate();
            validationResult.Combine(region.Validate());

            if(validationResult.Passed)
            {
                // Push to upstream data repository
                return await this._messageRepo.InsertAsync(message, region, cancellationToken);
            }
            else
            {
                throw new RequestValidationFailedException(validationResult);
            }
        }

        /// <inheritdoc/>
        public async Task<string> PublishAsync(SelfReportRequest request, long timeAtRequest, CancellationToken cancellationToken = default)
        {
            // Validate inputs
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            RequestValidationResult validationResult = request.Validate();
            validationResult.Combine(Validator.ValidateTimestamp(timeAtRequest));

            if(validationResult.Passed)
            {
                // Build MatchMessage from submitted content
                MatchMessage message = new MatchMessage();
                message.BluetoothSeeds.AddRange(request.Seeds);

                // Store in data repository
                return await this._messageRepo.InsertAsync(message, request.Region, cancellationToken);
            }
            else
            {
                throw new RequestValidationFailedException(validationResult);
            }
        }
    }
}
