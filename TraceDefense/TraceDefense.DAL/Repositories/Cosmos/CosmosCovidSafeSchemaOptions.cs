using Microsoft.Azure.Cosmos;
using TraceDefense.Entities;

namespace TraceDefense.DAL.Repositories.Cosmos
{
    /// <summary>
    /// Settings object for using Cosmos with the CovidSafe schema
    /// </summary>
    public class CosmosCovidSafeSchemaOptions
    {
        /// <summary>
        /// Cosmos target database name
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// Name of <see cref="Container"/> used to store <see cref="Query"/> objects
        /// </summary>
        public string QueryContainerName { get; set; }
        /// <summary>
        /// Creates a new <see cref="CosmosCovidSafeSchemaOptions"/> instance
        /// </summary>
        public CosmosCovidSafeSchemaOptions()
        {
        }
    }
}
