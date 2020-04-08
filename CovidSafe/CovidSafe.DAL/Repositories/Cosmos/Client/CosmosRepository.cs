using System;

using Microsoft.Azure.Cosmos;

namespace CovidSafe.DAL.Repositories.Cosmos.Client
{
    /// <summary>
    /// Base Cosmos-connected repository class
    /// </summary>
    public abstract class CosmosRepository
    {
        /// <summary>
        /// <see cref="CosmosContext"/> object used for database interaction
        /// </summary>
        public CosmosContext Context { get; private set; }
        /// <summary>
        /// <see cref="Container"/> used by the inheriting <see cref="CosmosRepository"/>
        /// </summary>
        public Container Container { get; protected set; }

        /// <summary>
        /// Creates a new <see cref="CosmosRepository"/> instance
        /// </summary>
        /// <param name="dbContext"><see cref="CosmosContext"/></param>
        public CosmosRepository(CosmosContext dbContext)
        {
            // Set local variables
            this.Context = dbContext;
        }
    }
}