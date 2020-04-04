using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace CovidSafe.DAL.Repositories.Cosmos
{
    /// <summary>
    /// Helper class for creating a Cosmos account connection
    /// </summary>
    public class CosmosConnectionFactory
    {
        /// <summary>
        /// CosmosDB Connection String
        /// </summary>
        private string _connectionString;
        /// <summary>
        /// Cosmos Client object
        /// </summary>
        public CosmosClient Client;

        /// <summary>
        /// Creates a new <see cref="CosmosConnectionFactory"/> instance
        /// </summary>
        /// <param name="appConfig">Application configuration</param>
        public CosmosConnectionFactory(IConfiguration appConfig)
        {
            // Get Cosmos connection data from application configuration
            this._connectionString = appConfig["CosmosConnection"];

            // Define client connection options
            CosmosClientOptions options = new CosmosClientOptions
            {
                // Enable multi-master/homing to allow region-specific connections
                ApplicationRegion = Regions.EastUS
            };

            // Create client object
            this.Client = new CosmosClient(this._connectionString, options);
        }
    }
}
