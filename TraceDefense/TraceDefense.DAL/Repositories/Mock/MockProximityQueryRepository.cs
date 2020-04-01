using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TraceDefense.DAL.Providers;
using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Repositories.Mock
{
    /// <summary>
    /// Mock implemnentation of <see cref="IProximityQueryRepository"/>
    /// </summary>
    public class MockProximityQueryRepository : IProximityQueryRepository
    {
        /// <summary>
        /// Mock object storage
        /// </summary>
        private Dictionary<string, ProximityQuery> _storage = new Dictionary<string, ProximityQuery>();

        /// <summary>
        /// Creates a new <see cref="MockProximityQueryRepository"/> instance
        /// </summary>
        public MockProximityQueryRepository()
        {
            // Create mock objects
            ProximityQuery query1 = new ProximityQuery
            {
                MessageVersion = 1
            };

            IDMatch idMatch1 = new IDMatch();
            idMatch1.Ids.Add("ABCD123456");
            GeoProximity geoMatch1 = new GeoProximity
            {
                DurationToleranceSecs = 600,
                ProximityRadiusMeters = 1000,
                UserMessage = "If you were at Trader Joe's on 128th St, please self-quarantine."
            };
            geoMatch1.Locations.Add(new LocationTime
            {
                Location = new Location
                {
                    Lattitude = -39.1231f,
                    Longitude = 47.1231f
                },
                Time = UtcTimeHelper.ToUtcTime(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
            });

            query1.IdList.Add(idMatch1);
            query1.GeoProximity.Add(geoMatch1);

            this._storage["00000000-0000-0000-0000-000000000000"] = query1;
        }

        /// <inheritdoc/>
        public Task<ProximityQuery> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            if(!String.IsNullOrEmpty(id))
            {
                if(this._storage.ContainsKey(id))
                {
                    return Task.FromResult(this._storage[id]);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(id));
            }
        }

        /// <inheritdoc/>
        public Task<IEnumerable<QueryInfo>> GetLatestAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            // For this mock instance, just get latest values
            IEnumerable<QueryInfo> info = this._storage.Keys.Select(v => new QueryInfo
            {
                QueryId = v,
                QueryTimestamp = UtcTimeHelper.ToUtcTime(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
            });

            return Task.FromResult(info);
        }

        /// <inheritdoc/>
        public Task<long> GetLatestRegionSizeAsync(string regionId, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            return Task.FromResult((long)1024);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<ProximityQuery>> GetRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
        {
            // Get all matching IDs
            List<ProximityQuery> matches = new List<ProximityQuery>();

            foreach(string id in ids)
            {
                if(this._storage.ContainsKey(id))
                {
                    matches.Add(this._storage[id]);
                }
            }

            return Task.FromResult((IEnumerable<ProximityQuery>)matches);
        }

        /// <inheritdoc/>
        public Task<string> InsertAsync(ProximityQuery record, CancellationToken cancellationToken = default)
        {
            // Create record with new ID
            string id = Guid.NewGuid().ToString();
            this._storage[id] = record;
            return Task.FromResult(id);
        }
    }
}
