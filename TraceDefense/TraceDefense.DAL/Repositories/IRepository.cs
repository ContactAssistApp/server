using System.Threading;
using System.Threading.Tasks;

namespace TraceDefense.DAL.Repositories
{
    /// <summary>
    /// Base data repository interface
    /// </summary>
    /// <typeparam name="T">Type of object served by this repository</typeparam>
    /// <typeparam name="TT">Type of object's primary key</typeparam>
    public interface IRepository<T, TT>
    {
        /// <summary>
        /// Retrieves a specific object from the repository
        /// </summary>
        /// <param name="id">Unique object identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Matching object or null</returns>
        Task<T> GetAsync(TT id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Store a new object in the repository
        /// </summary>
        /// <param name="record">Object to store</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Unique identifier of stored object</returns>
        Task<TT> InsertAsync(T record, CancellationToken cancellationToken = default);
    }
}
