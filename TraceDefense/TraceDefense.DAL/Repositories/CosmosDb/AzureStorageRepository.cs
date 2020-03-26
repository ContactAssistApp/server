using TraceDefense.DAL.Providers;

namespace TraceDefense.DAL.Repositories.CosmosDb
{
    /// <summary>
    /// CosmosDB Table Repository base class
    /// </summary>
    public abstract class AzureStorageRepository
    {
        /// <summary>
        /// Storage Account Connection Factory object
        /// </summary>
        protected readonly AzureStorageConnectionFactory ConnectionFactory;

        /// <summary>
        /// Creates a new <see cref="AzureStorageRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Storage account connection factory instance</param>
        public AzureStorageRepository(AzureStorageConnectionFactory connectionFactory)
        {
            this.ConnectionFactory = connectionFactory;
        }
    }
}
