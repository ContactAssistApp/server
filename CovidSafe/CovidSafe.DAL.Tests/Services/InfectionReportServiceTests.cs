using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.DAL.Repositories;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CovidSafe.DAL.Tests.Services
{
    /// <summary>
    /// Unit Tests for the <see cref="InfectionReportService"/> class
    /// </summary>
    [TestClass]
    public class InfectionReportServiceTests
    {
        /// <summary>
        /// Mock <see cref="IInfectionReportRepository"/> instance
        /// </summary>
        private Mock<IInfectionReportRepository> _repo;
        /// <summary>
        /// <see cref="IInfectionReportService"/> implementation being tested
        /// </summary>
        private InfectionReportService _service;

        /// <summary>
        /// Initializes a new <see cref="InfectionReportServiceTests"/> instance
        /// </summary>
        public InfectionReportServiceTests()
        {
            // Initialize repositories
            this._repo = new Mock<IInfectionReportRepository>();

            // Create service
            this._service = new InfectionReportService(this._repo.Object);
        }

        /// <summary>
        /// <see cref="InfectionReportService.GetByIdsAsync(IEnumerable{string}, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with empty 'ids' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetByIdsAsync_ArgumentNullOnEmptyParameters()
        {
            // Arrange
            IEnumerable<string> ids = new List<string>();

            // Act
            IEnumerable<InfectionReport> result = await this._service
                .GetByIdsAsync(ids, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="InfectionReportService.GetByIdsAsync(IEnumerable{string}, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with null 'ids' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetByIdsAsync_ArgumentNullOnNullParameters()
        {
            // Arrange
            // N/A

            // Act
            IEnumerable<InfectionReport> result = await this._service
                .GetByIdsAsync(null, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="InfectionReportService.GetByIdsAsync(IEnumerable{string}, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with empty 'ids' parameter
        /// </summary>
        [TestMethod]
        public async Task GetByIdsAsync_ReturnsMultipleMessages()
        {
            // Arrange
            IEnumerable<string> ids = new List<string>
            {
                "00000000-0000-0000-0000-000000000001",
                "00000000-0000-0000-0000-000000000002"
            };
            IEnumerable<InfectionReport> serviceResponse = new List<InfectionReport>
            {
                new InfectionReport(),
                new InfectionReport()
            };

            this._repo
                .Setup(r => r.GetRangeAsync(It.IsAny<IEnumerable<string>>(), CancellationToken.None))
                .Returns(Task.FromResult(serviceResponse));

            // Act
            IEnumerable<InfectionReport> result = await this._service
                .GetByIdsAsync(ids, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ids.Count(), result.Count());
        }

        /// <summary>
        /// <see cref="InfectionReportService.GetLatestInfoAsync(Region, long, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with empty 'region' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetLatestInfoAsync_ArgumentNullOnEmptyRegion()
        {
            // Arrange
            // N/A

            // Act
            IEnumerable<InfectionReportMetadata> result = await this._service
                .GetLatestInfoAsync(null, 0, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="InfectionReportService.GetLatestInfoAsync(Region, long, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with null 'region' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetLatestInfoAsync_ArgumentNullOnNullRegion()
        {
            // Arrange
            // N/A

            // Act
            IEnumerable<InfectionReportMetadata> result = await this._service
                .GetLatestInfoAsync(null, 0, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="InfectionReportService.GetLatestInfoAsync(Region, long, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with empty 'ids' parameter
        /// </summary>
        [TestMethod]
        public async Task GetLatestInfoAsync_ReturnsMultipleMessages()
        {
            // Arrange
            Region region = new Region
            {
                LatitudePrefix = 10.1234,
                LongitudePrefix = -10.1234,
                Precision = 4
            };
            IEnumerable<InfectionReportMetadata> serviceResponse = new List<InfectionReportMetadata>
            {
                new InfectionReportMetadata
                {
                    Id = "00000000-0000-0000-0000-000000000001",
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                },
                new InfectionReportMetadata
                {
                    Id = "00000000-0000-0000-0000-000000000002",
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                },
            };

            this._repo
                .Setup(r => r.GetLatestAsync(It.IsAny<Region>(), It.IsAny<long>(), CancellationToken.None))
                .Returns(Task.FromResult(serviceResponse));

            // Act
            IEnumerable<InfectionReportMetadata> result = await this._service
                .GetLatestInfoAsync(region, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(serviceResponse.Count(), result.Count());
        }

        /// <summary>
        /// <see cref="InfectionReportService.GetLatestRegionDataSizeAsync(Region, long, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with empty 'region' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetLatestRegionDataSizeAsync_ArgumentNullOnEmptyRegion()
        {
            // Arrange
            // N/A

            // Act
            long result = await this._service
                .GetLatestRegionDataSizeAsync(null, 0, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="InfectionReportService.GetLatestRegionDataSizeAsync(Region, long, CancellationToken)"/>
        /// throws <see cref="ArgumentNullException"/> with null 'region' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetLatestRegionDataSizeAsync_ArgumentNullOnNullRegion()
        {
            // Arrange
            // N/A

            // Act
            long result = await this._service
                .GetLatestRegionDataSizeAsync(null, 0, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="InfectionReportService.GetLatestRegionDataSizeAsync(Region, long, CancellationToken)"/> 
        /// returns a size of type <see cref="long"/>
        /// </summary>
        [TestMethod]
        public async Task GetLatestRegionDataSizeAsync_ReturnsSize()
        {
            // Arrange
            Region region = new Region
            {
                LatitudePrefix = 10.1234,
                LongitudePrefix = -10.1234,
                Precision = 4
            };
            long expectedResult = 1024;
            this._repo
                .Setup(r => r.GetLatestRegionSizeAsync(It.IsAny<Region>(), It.IsAny<long>(), CancellationToken.None))
                .Returns(Task.FromResult(expectedResult));

            // Act
            long result = await this._service.GetLatestRegionDataSizeAsync(
                region,
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                CancellationToken.None
            );

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        /// <summary>
        /// <see cref="InfectionReportService.PublishAreaAsync(AreaReport, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with null 'areas' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task PublishAreaAsync_ArgumentNullOnNullArea()
        {
            // Arrange
            // N/A

            // Act
            await this._service.PublishAreaAsync(null, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="InfectionReportService.PublishAreaAsync(AreaReport, CancellationToken)"/> 
        /// completes successfully with valid inputs
        /// </summary>
        [TestMethod]
        public async Task PublishAreaAsync_SucceedsOnValidInputs()
        {
            // Arrange
            AreaReport request = new AreaReport();
            request.Areas.Add(new InfectionArea
            {
                BeginTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                EndTimestamp = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeMilliseconds(),
                Location = new Coordinates
                {
                    Latitude = 10.1234,
                    Longitude = -10.1234
                },
                RadiusMeters = 100
            });
            request.UserMessage = "Test user message!";
            string repoResponse = "00000000-0000-0000-0000-0000000000001";

            this._repo
                .Setup(r => r.InsertAsync(It.IsAny<InfectionReport>(), It.IsAny<Region>(), CancellationToken.None))
                .Returns(Task.FromResult(repoResponse));

            // Act
            await this._service.PublishAreaAsync(request, CancellationToken.None);

            // Assert
            // No exceptions thrown
        }

        /// <summary>
        /// <see cref="InfectionReportService.PublishAsync(InfectionReport, Region, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with null 'message' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task PublishAsync_ArgumentNullOnNullInfectionReport()
        {
            // Arrange
            Region region = new Region
            {
                LatitudePrefix = 10.1234,
                LongitudePrefix = -10.1234,
                Precision = 4
            };

            // Act
            string result = await this._service
                .PublishAsync(null, region, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="InfectionReportService.PublishAsync(InfectionReport, Region, CancellationToken)"/> 
        /// succeeds with valid inputs
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task PublishAsync_ArgumentNullOnNullRegion()
        {
            // Arrange
            InfectionReport request = new InfectionReport();
            AreaReport areaMatch = new AreaReport
            {
                UserMessage = "Test user message"
            };
            areaMatch.Areas.Add(new InfectionArea
            {
                BeginTimestamp = 0,
                EndTimestamp = 1,
                Location = new Coordinates
                {
                    Latitude = 10.1234,
                    Longitude = -10.1234
                },
                RadiusMeters = 100
            });
            request.AreaReports.Add(areaMatch);
            request.BluetoothSeeds.Add(new BluetoothSeed
            {
                EndTimestamp = 1,
                BeginTimestamp = 0,
                Seed = "00000000-0000-0000-0000-000000000000"
            });

            // Act
            string result = await this._service
                .PublishAsync(request, null, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="InfectionReportService.PublishAsync(InfectionReport, Region, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with null parameters
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task PublishAsync_ArgumentNullOnNullRequest()
        {
            // Arrange
            // N/A

            // Act
            string result = await this._service
                .PublishAsync(null, null, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="InfectionReportService.PublishAsync(InfectionReport, Region, CancellationToken)"/> 
        /// succeeds with valid inputs
        /// </summary>
        [TestMethod]
        public async Task PublishAsync_SucceedsOnValidMessage()
        {
            // Arrange
            string repoResponse = "00000000-0000-0000-0000-000000000002";
            Region region = new Region
            {
                LatitudePrefix = 10.1234,
                LongitudePrefix = -10.1234,
                Precision = 4
            };
            InfectionReport request = new InfectionReport();
            AreaReport areaReport = new AreaReport
            {
                UserMessage = "Test user message"
            };
            areaReport.Areas.Add(new InfectionArea
            {
                BeginTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                EndTimestamp = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeMilliseconds(),
                Location = new Coordinates
                {
                    Latitude = 10.1234,
                    Longitude = -10.1234
                },
                RadiusMeters = 100
            });
            request.AreaReports.Add(areaReport);
            request.BluetoothSeeds.Add(new BluetoothSeed
            {
                BeginTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                EndTimestamp = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeMilliseconds(),
                Seed = "00000000-0000-0000-0000-000000000001"
            });

            this._repo
                .Setup(r => r.InsertAsync(It.IsAny<InfectionReport>(), It.IsAny<Region>(), CancellationToken.None))
                .Returns(Task.FromResult(repoResponse));

            // Act
            string result = await this._service
                .PublishAsync(request, region, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(repoResponse, result);
        }
    }
}
