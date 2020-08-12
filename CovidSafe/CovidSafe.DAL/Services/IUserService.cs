using CovidSafe.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace CovidSafe.DAL.Services
{
    /// <summary>
    /// <see cref="User"/> service layer definition
    /// </summary>
    public interface IUserService : IService
    {
        /// <summary>
        /// Retrieves <see cref="User"/> object by credentials
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="password">Password</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="User"/> object</returns>
        Task<User> Authenticate(string username, string password, CancellationToken cancellationToken = default);
    }
}
