using CovidSafe.Entities.Protos;
using Microsoft.Azure.Cosmos;

namespace CovidSafe.DAL.Repositories.Cosmos.Client
{
    /// <summary>
    /// Settings object for using Cosmos with the CovidSafe schema
    /// </summary>
    public class CosmosSchemaConfigurationSection
    {
        /// <summary>
        /// Cosmos target database name
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// Maximum age of data to return in queries, in number of days
        /// </summary>
        public int MaxDataAgeToReturnDays { get; set; }
        /// <summary>
        /// Name of <see cref="Container"/> used to store <see cref="MatchMessage"/> objects
        /// </summary>
        public string MessageContainerName { get; set; }

        /// <summary>
        /// Creates a new <see cref="CosmosSchemaConfigurationSection"/> instance
        /// </summary>
        public CosmosSchemaConfigurationSection()
        {
        }
    }
}
