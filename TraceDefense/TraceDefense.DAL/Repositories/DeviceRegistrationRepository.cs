using System;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.Entities.Registration;

namespace TraceDefense.DAL.Repositories
{
    /// <summary>
    /// In-memory <see cref="DeviceRegistration"/> repository implementation
    /// </summary>
    public class DeviceRegistrationRepository : IDeviceRegistrationRepository
    {
        public Task RegisterAsync(string deviceId, string region, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
