using Microsoft.Azure.Cosmos;
using TraceDefense.DAL.Providers;

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
        /// Name of database used by all implementing repositories
        /// </summary>
        public const string DATABASE_NAME = "traces";

        /// <summary>
        /// Creates a new <see cref="CosmosRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory"></param>
        public CosmosRepository(CosmosConnectionFactory connectionFactory)
        {
            this.ConnectionFactory = connectionFactory;
            this.Database = this.ConnectionFactory.Client.GetDatabase(DATABASE_NAME);
        }
    }
}
