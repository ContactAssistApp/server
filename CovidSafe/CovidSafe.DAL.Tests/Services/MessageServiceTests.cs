using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.DAL.Repositories;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Protos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CovidSafe.DAL.Tests.Services
{
    /// <summary>
    /// Unit Tests for the <see cref="MessageService"/> class
    /// </summary>
    [TestClass]
    public class MessageServiceTests
    {
        /// <summary>
        /// Mock <see cref="IMatchMessageRepository"/> instance
        /// </summary>
        private Mock<IMatchMessageRepository> _repo;
        /// <summary>
        /// <see cref="IMessageService"/> implementation being tested
        /// </summary>
        private MessageService _service;

        /// <summary>
        /// Initializes a new <see cref="MessageServiceTests"/> instance
        /// </summary>
        public MessageServiceTests()
        {
            // Initialize repositories
            this._repo = new Mock<IMatchMessageRepository>();

            // Create service
            this._service = new MessageService(this._repo.Object);
        }

        /// <summary>
        /// <see cref="MessageService.GetByIdsAsync(IEnumerable{string}, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with empty 'ids' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetByIdsAsync_ArgumentNullOnEmptyParameters()
        {
            // Arrange
            IEnumerable<string> ids = new List<string>();

            // Act
            IEnumerable<MatchMessage> result = await this._service
                .GetByIdsAsync(ids, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="MessageService.GetByIdsAsync(IEnumerable{string}, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with null 'ids' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetByIdsAsync_ArgumentNullOnNullParameters()
        {
            // Arrange
            // N/A

            // Act
            IEnumerable<MatchMessage> result = await this._service
                .GetByIdsAsync(null, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="MessageService.GetByIdsAsync(IEnumerable{string}, CancellationToken)"/> 
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
            IEnumerable<MatchMessage> serviceResponse = new List<MatchMessage>
            {
                new MatchMessage(),
                new MatchMessage()
            };

            this._repo
                .Setup(r => r.GetRangeAsync(It.IsAny<IEnumerable<string>>(), CancellationToken.None))
                .Returns(Task.FromResult(serviceResponse));

            // Act
            IEnumerable<MatchMessage> result = await this._service
                .GetByIdsAsync(ids, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ids.Count(), result.Count());
        }

        /// <summary>
        /// <see cref="MessageService.GetLatestInfoAsync(Region, long, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with empty 'region' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetLatestInfoAsync_ArgumentNullOnEmptyRegion()
        {
            // Arrange
            // N/A

            // Act
            IEnumerable<MessageInfo> result = await this._service
                .GetLatestInfoAsync(null, 0, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="MessageService.GetLatestInfoAsync(Region, long, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with null 'region' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetLatestInfoAsync_ArgumentNullOnNullRegion()
        {
            // Arrange
            // N/A

            // Act
            IEnumerable<MessageInfo> result = await this._service
                .GetLatestInfoAsync(null, 0, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="MessageService.GetLatestInfoAsync(Region, long, CancellationToken)"/> 
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
            IEnumerable<MessageInfo> serviceResponse = new List<MessageInfo>
            {
                new MessageInfo
                {
                    MessageId = "00000000-0000-0000-0000-000000000001",
                    MessageTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                },
                new MessageInfo
                {
                    MessageId = "00000000-0000-0000-0000-000000000002",
                    MessageTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                },
            };

            this._repo
                .Setup(r => r.GetLatestAsync(It.IsAny<Region>(), It.IsAny<long>(), CancellationToken.None))
                .Returns(Task.FromResult(serviceResponse));

            // Act
            IEnumerable<MessageInfo> result = await this._service
                .GetLatestInfoAsync(region, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(serviceResponse.Count(), result.Count());
        }

        /// <summary>
        /// <see cref="MessageService.GetLatestRegionDataSizeAsync(Region, long, CancellationToken)"/> 
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
        /// <see cref="MessageService.GetLatestRegionDataSizeAsync(Region, long, CancellationToken)"/>
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
        /// <see cref="MessageService.GetLatestRegionDataSizeAsync(Region, long, CancellationToken)"/> 
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
        /// <see cref="MessageService.PublishAreaAsync(AreaMatch, CancellationToken)"/> 
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
        /// <see cref="MessageService.PublishAreaAsync(AreaMatch, CancellationToken)"/> 
        /// completes successfully with valid inputs
        /// </summary>
        [TestMethod]
        public async Task PublishAreaAsync_SucceedsOnValidInputs()
        {
            // Arrange
            AreaMatch request = new AreaMatch();
            request.Areas.Add(new Area
            {
                BeginTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                EndTime = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeMilliseconds(),
                Location = new Location
                {
                    Latitude = 10.1234,
                    Longitude = -10.1234
                },
                RadiusMeters = 100
            });
            request.UserMessage = "Test user message!";
            string repoResponse = "00000000-0000-0000-0000-0000000000001";

            this._repo
                .Setup(r => r.InsertAsync(It.IsAny<MatchMessage>(), It.IsAny<Region>(), CancellationToken.None))
                .Returns(Task.FromResult(repoResponse));

            // Act
            await this._service.PublishAreaAsync(request, CancellationToken.None);

            // Assert
            // No exceptions thrown
        }

        /// <summary>
        /// <see cref="MessageService.PublishAsync(MatchMessage, Region, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with null 'message' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task PublishAsync_ArgumentNullOnNullMatchMessage()
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
        /// <see cref="MessageService.PublishAsync(MatchMessage, Region, CancellationToken)"/> 
        /// succeeds with valid inputs
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task PublishAsync_ArgumentNullOnNullRegion()
        {
            // Arrange
            MatchMessage request = new MatchMessage();
            AreaMatch areaMatch = new AreaMatch
            {
                UserMessage = "Test user message"
            };
            areaMatch.Areas.Add(new Area
            {
                BeginTime = 0,
                EndTime = 1,
                Location = new Location
                {
                    Latitude = 10.1234,
                    Longitude = -10.1234
                },
                RadiusMeters = 100
            });
            request.AreaMatches.Add(areaMatch);
            request.BluetoothSeeds.Add(new BlueToothSeed
            {
                Seed = "00000000-0000-0000-0000-000000000000",
                SequenceEndTime = 1,
                SequenceStartTime = 0
            });

            // Act
            string result = await this._service
                .PublishAsync(request, null, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="MessageService.PublishAsync(SelfReportRequest, long, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with null 'request' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task PublishAsync_ArgumentNullOnNullRequest()
        {
            // Arrange
            // N/A

            // Act
            string result = await this._service
                .PublishAsync(null, 0, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="MessageService.PublishAsync(MatchMessage, Region, CancellationToken)"/> 
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
            MatchMessage request = new MatchMessage();
            AreaMatch areaMatch = new AreaMatch
            {
                UserMessage = "Test user message"
            };
            areaMatch.Areas.Add(new Area
            {
                BeginTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                EndTime = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeMilliseconds(),
                Location = new Location
                {
                    Latitude = 10.1234,
                    Longitude = -10.1234
                },
                RadiusMeters = 100
            });
            request.AreaMatches.Add(areaMatch);
            request.BluetoothSeeds.Add(new BlueToothSeed
            {
                Seed = "00000000-0000-0000-0000-000000000001",
                SequenceEndTime = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeMilliseconds(),
                SequenceStartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });

            this._repo
                .Setup(r => r.InsertAsync(It.IsAny<MatchMessage>(), It.IsAny<Region>(), CancellationToken.None))
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
