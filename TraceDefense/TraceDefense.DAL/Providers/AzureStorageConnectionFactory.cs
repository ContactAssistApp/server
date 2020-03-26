using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace TraceDefense.DAL.Providers
{
    /// <summary>
    /// Helper class for creating an Azure Storage Account connection
    /// </summary>
    public class AzureStorageConnectionFactory
    {
        /// <summary>
        /// Application configuration
        /// </summary>
        private IConfiguration _appConfig;
        /// <summary>
        /// Cloud Storage Account object
        /// </summary>
        public CloudStorageAccount StorageAccount;

        /// <summary>
        /// Creates a new <see cref="AzureStorageConnectionFactory"/> instance
        /// </summary>
        /// <param name="appConfig">Application configuration</param>
        public AzureStorageConnectionFactory(IConfiguration appConfig)
        {
            this._appConfig = appConfig;

            // Create cloud table client from application configuration
            string connectionString = this._appConfig.GetConnectionString("StorageConnectionString");
            this.StorageAccount = CloudStorageAccount.Parse(connectionString);
        }
    }
}
