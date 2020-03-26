using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TraceDefense.DAL.Repositories;

namespace TraceDefence.Core
{
    class DeviceRegistrationRepository : IDeviceRegistrationRepository
    {
        public Task RegisterAsync(string deviceId, string region, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
