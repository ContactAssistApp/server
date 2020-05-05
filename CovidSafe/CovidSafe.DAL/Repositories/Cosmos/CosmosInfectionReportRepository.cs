using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.DAL.Helpers;
using CovidSafe.DAL.Repositories.Cosmos.Client;
using CovidSafe.DAL.Repositories.Cosmos.Records;
using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Reports;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Cosmos.Spatial;

namespace CovidSafe.DAL.Repositories.Cosmos
{
    /// <summary>
    /// CosmosDB implementation of <see cref="IInfectionReportRepository"/>
    /// </summary>
    public class CosmosInfectionReportRepository : CosmosRepository, IInfectionReportRepository
    {
        /// <summary>
        /// Precision of regions that are used for keys.
        /// </summary>
        private int RegionPrecision = 4;
        /// <summary>
        /// Search extension size
        /// </summary>
        private int RegionsExtension = 1;

        /// <summary>
        /// Creates a new <see cref="CosmosInfectionReportRepository"/> instance
        /// </summary>
        /// <param name="dbContext"><see cref="CosmosContext"/> instance</param>
        public CosmosInfectionReportRepository(CosmosContext dbContext) : base(dbContext)
        {
            // Create container reference
            this.Container = this.Context.GetContainer(
                this.Context.SchemaOptions.MessageContainerName
            );
        }

        /// <summary>
        /// Returns the most restrictive timestamp filter, based on the application 
        /// configuration and the one provided by the user
        /// </summary>
        /// <param name="timestampFilter">Original timestamp filter applied to query</param>
        /// <returns>Timestamp filter value, in ms since UNIX epoch</returns>
        private long _getTimestampFilter(long timestampFilter)
        {
            // Get the default timestamp filter value
            long defaultFilter = DateTimeOffset.UtcNow
                .AddDays(-(this.Context.SchemaOptions.MaxDataAgeToReturnDays))
                .ToUnixTimeMilliseconds();

            // If a timestamp filter was provided for the query, see if that one is more restrictive than ours
            if(timestampFilter > 0)
            {
                // Take most restrictive timestamp filter
                return Math.Max(defaultFilter, timestampFilter);
            }
            else
            {
                // Use our filter by default, if none was provided already for the query
                return defaultFilter;
            }
        }

        /// <inheritdoc/>
        public async Task<InfectionReport> GetAsync(string messageId, CancellationToken cancellationToken = default)
        {
            // Create LINQ query
            var queryable = this.Container
                .GetItemLinqQueryable<InfectionReportRecord>();

            // Execute query
            var iterator = queryable
                .Where(r =>
                    r.Id == messageId
                    && r.Version == InfectionReportRecord.CURRENT_RECORD_VERSION
                )
                .Select(r => r.Value)
                .ToFeedIterator();

            List<InfectionReport> results = new List<InfectionReport>();

            while (iterator.HasMoreResults)
            {
                results.AddRange(await iterator.ReadNextAsync());
            }

            return results.FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<InfectionReportMetadata>> GetLatestAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            // Get boundaries for provided region
            Point regionPoint = new Point(region.LongitudePrefix, region.LatitudePrefix);

            // Create LINQ query
            var queryable = this.Container
                .GetItemLinqQueryable<InfectionReportRecord>();

            RegionBoundary rb = RegionHelper.GetConnectedRegionsRange(region, this.RegionsExtension, this.RegionPrecision);
            long timeStampFilter = this._getTimestampFilter(lastTimestamp);

            // Execute query
            var iterator = queryable
                .Where(r =>
                    r.Timestamp > timeStampFilter
                    && r.RegionBoundary.Min.Latitude >= rb.Min.Latitude
                    && r.RegionBoundary.Min.Latitude <= rb.Max.Latitude
                    && r.RegionBoundary.Min.Longitude >= rb.Min.Longitude
                    && r.RegionBoundary.Min.Longitude <= rb.Max.Longitude
                    && r.Version == InfectionReportRecord.CURRENT_RECORD_VERSION
                )
                .Select(r => new InfectionReportMetadata
                {
                    Id = r.Id,
                    Timestamp = r.Timestamp
                })
                .ToFeedIterator();

            List<InfectionReportMetadata> results = new List<InfectionReportMetadata>();

            while(iterator.HasMoreResults)
            {
                results.AddRange(await iterator.ReadNextAsync());
            }

            return results;
        }

        /// <inheritdoc/>
        public async Task<long> GetLatestRegionSizeAsync(Region region, long lastTimestamp, CancellationToken cancellationToken = default)
        {
            // Get boundaries for provided region
            Point regionPoint = new Point(region.LongitudePrefix, region.LatitudePrefix);

            // Create LINQ query
            var queryable = this.Container
                .GetItemLinqQueryable<InfectionReportRecord>();

            RegionBoundary rb = RegionHelper.GetConnectedRegionsRange(region, this.RegionsExtension, this.RegionPrecision);
            long timeStampFilter = this._getTimestampFilter(lastTimestamp);

            // Execute query
            var size = await queryable
                .Where(r =>
                    r.Timestamp > timeStampFilter
                    && r.RegionBoundary.Min.Latitude >= rb.Min.Latitude
                    && r.RegionBoundary.Min.Latitude <= rb.Max.Latitude
                    && r.RegionBoundary.Min.Longitude >= rb.Min.Longitude
                    && r.RegionBoundary.Min.Longitude <= rb.Max.Longitude
                    && r.Version == InfectionReportRecord.CURRENT_RECORD_VERSION
                )
                .Select(r => r.Size)
                .SumAsync(cancellationToken)
                .ConfigureAwait(false);

            return size;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<InfectionReport>> GetRangeAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
        {
            // Create LINQ query
            var queryable = this.Container
                .GetItemLinqQueryable<InfectionReportRecord>();

            // Execute query
            var iterator = queryable
                .Where(r =>
                    ids.Contains(r.Id)
                    && r.Version == InfectionReportRecord.CURRENT_RECORD_VERSION
                )
                .Select(r => r.Value)
                .ToFeedIterator();

            List<InfectionReport> results = new List<InfectionReport>();

            while (iterator.HasMoreResults)
            {
                results.AddRange(await iterator.ReadNextAsync());
            }

            return results;
        }

        /// <inheritdoc/>
        public async Task<string> InsertAsync(InfectionReport report, Region region, CancellationToken cancellationToken = default)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            region = RegionHelper.AdjustToPrecision(region);

            // Get allowed region boundary
            RegionBoundary boundary = RegionHelper.GetRegionBoundary(region);

            var record = new InfectionReportRecord(report)
            {
                RegionBoundary = new RegionBoundaryProperty(boundary),
                Region = new RegionProperty(region),
                PartitionKey = InfectionReportRecord.GetPartitionKey(region)
            };

            ItemResponse<InfectionReportRecord> response = await this.Container
                .CreateItemAsync<InfectionReportRecord>(
                    record,
                    new PartitionKey(record.PartitionKey),
                    cancellationToken: cancellationToken
                );

            return response.Resource.Id;
        }

        public async Task InsertAsync(InfectionReport report, IEnumerable<Region> regions, CancellationToken cancellationToken = default)
        {
            // Validate inputs
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            // Prepare records to insert (grouped by partition key)
            var recordGroups = regions.Select(
                r => new InfectionReportRecord(report)
                {
                    RegionBoundary = new RegionBoundaryProperty(
                        RegionHelper.GetRegionBoundary(r)),
                    Region = new RegionProperty(r),
                    PartitionKey = InfectionReportRecord.GetPartitionKey(r)
                }).GroupBy(r => r.PartitionKey);

            // Begin batch operation
            // All MatchMessageRecords will have same PartitionID in this batch
            var batches = recordGroups.Select(g => g.Aggregate(
                this.Container.CreateTransactionalBatch(new PartitionKey(g.Key)),
                (result, item) => result.CreateItem<InfectionReportRecord>(item)));

            // Execute transactions
            // TODO: make a single transaction. 
            var responses = await Task.WhenAll(batches.Select(b => b.ExecuteAsync(cancellationToken)));

            var failed = responses.Where(r => !r.IsSuccessStatusCode);
            if (failed.Any())
            {
                throw new Exception(
                    String.Format(
                        "{0} out of {1} insertions failed. Cosmos bulk insert failed with HTTP Status Code {2}.",
                        responses.Count(),
                        failed.Count(),
                        failed.First().StatusCode.ToString()
                )
                );
            }
        }
    }
}
