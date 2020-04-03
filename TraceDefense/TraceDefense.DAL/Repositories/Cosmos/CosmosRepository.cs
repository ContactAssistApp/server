using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace TraceDefense.DAL.Repositories.Cosmos
{
    /// <summary>
    /// Base Cosmos-connected repository class
    /// </summary>
    public abstract class CosmosRepository
    {
        /// <summary>
        /// <see cref="CosmosConnectionFactory"/> instance
        /// </summary>
        protected CosmosConnectionFactory ConnectionFactory;
        /// <summary>
        /// Primary database used by implementing repositories
        /// </summary>
        protected Database Database;
        /// <summary>
        /// Schema configuration object
        /// </summary>
        protected CosmosCovidSafeSchemaOptions SchemaOptions;

        /// <summary>
        /// Creates a new <see cref="CosmosRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Database connection factory instance</param>
        /// <param name="schemaOptions">Schema options object</param>
        public CosmosRepository(CosmosConnectionFactory connectionFactory, IOptionsMonitor<CosmosCovidSafeSchemaOptions> schemaOptions)
        {
            // Set local variables
            this.ConnectionFactory = connectionFactory;
            this.SchemaOptions = schemaOptions.CurrentValue;

            // Create Database object reference
            this.Database = this.ConnectionFactory.Client.GetDatabase(this.SchemaOptions.DatabaseName);
        }
    }
}
