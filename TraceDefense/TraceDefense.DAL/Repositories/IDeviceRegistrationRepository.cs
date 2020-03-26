using System.Threading;
using System.Threading.Tasks;

using TraceDefense.Entities.Registration;

namespace TraceDefense.DAL.Repositories
{
    /// <summary>
    /// <see cref="DeviceRegistration"/> repository definition
    /// </summary>
    public interface IDeviceRegistrationRepository : IRepository<DeviceRegistration>
    {
        /// <summary>
        /// Registers a device in the data repository
        /// </summary>
        /// <param name="deviceId">Unique device identifier</param>
        /// <param name="region">Device geographic region</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task RegisterAsync(string deviceId, string region, CancellationToken cancellationToken = default);
    }
}
