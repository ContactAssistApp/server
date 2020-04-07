using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace CovidSafe.DAL.Repositories.Cosmos.Client
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
        /// Default <see cref="CosmosClientOptions"/>
        /// </summary>
        public CosmosClientOptions DefaultClientOptions { get; private set; } = new CosmosClientOptions
        {
            ApplicationRegion = Regions.EastUS
        };

        /// <summary>
        /// Creates a new <see cref="CosmosConnectionFactory"/> instance
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        public CosmosConnectionFactory(string connectionString)
        {
            // Store local variables
            this._connectionString = connectionString;
        }

        /// <summary>
        /// Creates a new <see cref="CosmosClient"/> instance with default 
        /// <see cref="CosmosClientOptions"/>
        /// </summary>
        /// <returns><see cref="CosmosClient"/> instance</returns>
        public CosmosClient GetClient()
        {
            // Use overload for simplicity
            return this.GetClient(this.DefaultClientOptions);
        }

        /// <summary>
        /// Creates a new <see cref="CosmosClient"/> instance with a custom configuration
        /// </summary>
        /// <param name="options"><see cref="CosmosClientOptions"/></param>
        /// <returns><see cref="CosmosClient"/> instance</returns>
        public CosmosClient GetClient(CosmosClientOptions options)
        {
            return new CosmosClient(this._connectionString, options);
        }
    }
}
