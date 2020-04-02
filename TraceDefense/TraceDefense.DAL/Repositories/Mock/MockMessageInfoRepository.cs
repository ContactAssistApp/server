using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Repositories.Mock
{
    public class MockMessageInfoRepository : IMessageInfoRepository
    {
        public MockMessageInfoRepository()
        {
        }

        public Task<IEnumerable<MessageInfo>> GetLatestAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetLatestRegionSizeAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMessageInfoAsync(Region region, MessageInfo info, long size, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
