using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Spatial;
using Microsoft.Extensions.Options;
using TraceDefense.DAL.Providers;
using TraceDefense.DAL.Repositories.Cosmos.Records;
using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Repositories.Cosmos
{
    /// <summary>
    /// CosmosDB implementation of <see cref="IProximityQueryRepository"/>
    /// </summary>
    public class CosmosMessageRepository : CosmosRepository, IMessageRepository
    {
        /// <summary>
        /// <see cref="ProximityQuery"/> container object
        /// </summary>
        private Container _queryContainer;

        /// <summary>
        /// Creates a new <see cref="CosmosProximityQueryRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Database connection factory instance</param>
        /// <param name="schemaOptions">Schema options object</param>
        public CosmosMessageRepository(CosmosConnectionFactory connectionFactory, IOptionsMonitor<CosmosCovidSafeSchemaOptions> schemaOptions) : base(connectionFactory, schemaOptions)
        {
            // Create container reference
            this._queryContainer = this.Database
                .GetContainer(this.SchemaOptions.QueryContainerName);
        }

        /// <inheritdoc/>
        public async Task<MatchMessage> GetAsync(string messageId, CancellationToken cancellationToken = default)
        {
            // Build query
            string sqlQuery = "SELECT TOP 1 * FROM c WHERE c.messageId = @messageId";
            QueryDefinition queryDef = new QueryDefinition(sqlQuery)
                .WithParameter("@messageId", messageId);

            // Get results
            FeedIterator<MatchMessageRecord> iterator = this._queryContainer
                .GetItemQueryIterator<MatchMessageRecord>(queryDef);
            MatchMessageRecord instance = null;

            while (iterator.HasMoreResults)
            {
                FeedResponse<MatchMessageRecord> result = await iterator.ReadNextAsync(cancellationToken);
                instance = result.Resource.FirstOrDefault();
            }

            if (instance != null)
            {
                return instance.Value;
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MatchMessage>> GetRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
        {
            var messages = new List<MatchMessage> { new MatchMessage() };

            return messages;
        }

        /// <inheritdoc/>
        public async Task InsertAsync(Location locmin, Location locmax, MatchMessage message, CancellationToken cancellationToken = default)
        {
            // Create common properties for each new record
            string messageId = Guid.NewGuid().ToString();
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var record = new MatchMessageRecord(message)
            {
                MessageId = messageId,
                Timestamp = timestamp,
                Version = message.MatchProtocolVersion,
                LocMin = new Point(locmin.Longitude, locmin.Lattitude),
                LocMax = new Point(locmax.Longitude, locmax.Lattitude)
            };

            ItemResponse<MatchMessageRecord> response = await this._queryContainer
                .CreateItemAsync<MatchMessageRecord>(
                    record,
                    new PartitionKey(record.MessageId),
                    cancellationToken: cancellationToken
                );
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MessageInfo>> GetLatestAsync(Location locmin, Location locmax, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            //TODO: Implement
            return new List<MessageInfo>();
        }

        /// <inheritdoc/>
        public async Task<long> GetLatestRegionSizeAsync(Location locmin, Location locmax, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            // TODO: Implement
            return (long)100;
        }

    }
}
