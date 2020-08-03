using CovidSafe.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace CovidSafe.DAL.Services
{
    public class TestUserService : IUserService
    {
        /// <inheritdoc/>
        public async Task<User> Authenticate(string username, string password, CancellationToken cancellationToken)
        {
            if (username == "admin" && password == "password")
                return new User { Username = username };
            return null;
        }
    }
}
