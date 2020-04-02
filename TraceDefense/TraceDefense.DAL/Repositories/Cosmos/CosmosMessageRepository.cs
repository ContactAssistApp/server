using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Spatial;
using Microsoft.Extensions.Options;
using TraceDefense.DAL.Providers;
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
            return null;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MatchMessage>> GetRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
        {
            var messages = new List<MatchMessage> { new MatchMessage() };

            return messages;
        }

        /// <inheritdoc/>
        public async Task<string> InsertAsync(MatchMessage message, CancellationToken cancellationToken = default)
        {
            // Create common properties for each new record
            string messageId = Guid.NewGuid().ToString();

            return messageId;
        }
    }
}
