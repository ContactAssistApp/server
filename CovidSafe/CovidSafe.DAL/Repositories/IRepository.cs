using System.Threading;
using System.Threading.Tasks;

namespace CovidSafe.DAL.Repositories
{
    /// <summary>
    /// Base repository definition
    /// </summary>
    /// <typeparam name="T">Object type handled by repository implementation</typeparam>
    /// <typeparam name="TT">Type used by the primary key</typeparam>
    public interface IRepository<T, TT>
    {
        /// <summary>
        /// Deletes an object matching the provided identifier
        /// </summary>
        /// <param name="id">Unique object identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteAsync(TT id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves an object which matches the provided identifier
        /// </summary>
        /// <param name="id">Unique object identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Object or null</returns>
        Task<T> GetAsync(TT id, CancellationToken cancellationToken = default);
    }
}
