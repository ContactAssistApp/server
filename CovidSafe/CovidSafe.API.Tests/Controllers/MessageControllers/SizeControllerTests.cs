using System;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.API.Controllers.MessageControllers;
using CovidSafe.DAL.Repositories;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Protos;
using CovidSafe.Entities.Protos.Deprecated;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CovidSafe.API.Tests.Controllers.MessageControllers
{
    /// <summary>
    /// Unit Tests for the <see cref="SizeController"/> class
    /// </summary>
    [TestClass]
    public class SizeControllerTests
    {
        /// <summary>
        /// Test <see cref="SizeController"/> instance
        /// </summary>
        private SizeController _controller;
        /// <summary>
        /// Mock <see cref="IMatchMessageRepository"/> instance
        /// </summary>
        private Mock<IMatchMessageRepository> _repo;
        /// <summary>
        /// <see cref="MessageService"/> instance
        /// </summary>
        private MessageService _service;

        /// <summary>
        /// Creates a new <see cref="MessageControllerTests"/> instance
        /// </summary>
        public SizeControllerTests()
        {
            // Configure repo mock
            this._repo = new Mock<IMatchMessageRepository>();

            // Configure service
            this._service = new MessageService(this._repo.Object);

            // Configure controller
            this._controller = new SizeController(this._service);
            this._controller.ControllerContext = new ControllerContext();
            this._controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        /// <summary>
        /// <see cref="SizeController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with invalid timestamp
        /// </summary>
        [TestMethod]
        public async Task GetAsync_BadRequestObjectWithInvalidTimestamp()
        {
            // Arrange
            // N/A

            // Act
            // Min Latitude is -90
            ActionResult<MessageSizeResponse> controllerResponse = await this._controller
                .GetAsync(10.1234, -10.1234, 4, -1, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="SizeController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with too high Latitude
        /// </summary>
        [TestMethod]
        public async Task GetAsync_BadRequestObjectWithTooHighLatitude()
        {
            // Arrange
            // N/A

            // Act
            // Max Latitude is 90
            ActionResult<MessageSizeResponse> controllerResponse = await this._controller
                .GetAsync(91, -10.1234, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="SizeController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with too high Longitude
        /// </summary>
        [TestMethod]
        public async Task GetAsync_BadRequestObjectWithTooHighLongitude()
        {
            // Arrange
            // N/A

            // Act
            // Max Longitude is 180
            ActionResult<MessageSizeResponse> controllerResponse = await this._controller
                .GetAsync(10.1234, 181, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="SizeController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with too high Latitude
        /// </summary>
        [TestMethod]
        public async Task GetAsync_BadRequestObjectWithTooLowLatitude()
        {
            // Arrange
            // N/A

            // Act
            // Min Latitude is -90
            ActionResult<MessageSizeResponse> controllerResponse = await this._controller
                .GetAsync(-91, -10.1234, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="SizeController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with too low Longitude
        /// </summary>
        [TestMethod]
        public async Task GetAsync_BadRequestObjectWithTooLowLongitude()
        {
            // Arrange
            // N/A

            // Act
            // Min Longitude is -180
            ActionResult<MessageSizeResponse> controllerResponse = await this._controller
                .GetAsync(10.1234, -181, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="SizeController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="OkObjectResult"/> with matched parameters
        /// </summary>
        [TestMethod]
        public async Task GetAsync_OkWithMatchedParams()
        {
            // Arrange
            Region requestedRegion = new Region
            {
                LatitudePrefix = 10.1234,
                LongitudePrefix = -10.1234,
                Precision = 4
            };

            long expectedSize = 1234;

            this._repo
                .Setup(s => s.GetLatestRegionSizeAsync(
                    It.IsAny<Region>(),
                    It.IsAny<long>(),
                    CancellationToken.None
                ))
                .Returns(Task.FromResult(expectedSize));

            // Act
            ActionResult<MessageSizeResponse> controllerResponse = await this._controller
                .GetAsync(
                    requestedRegion.LatitudePrefix,
                    requestedRegion.LongitudePrefix,
                    requestedRegion.Precision,
                    DateTimeOffset.UtcNow.AddHours(-1).ToUnixTimeMilliseconds(),
                    CancellationToken.None
                );

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(OkObjectResult));
            OkObjectResult castedResult = controllerResponse.Result as OkObjectResult;
            Assert.IsInstanceOfType(castedResult.Value, typeof(MessageSizeResponse));
            MessageSizeResponse responseResult = castedResult.Value as MessageSizeResponse;
            Assert.AreEqual(expectedSize, responseResult.SizeOfQueryResponse);
        }

        /// <summary>
        /// <see cref="SizeController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="OkObjectResult"/> with unmatched parameters
        /// </summary>
        [TestMethod]
        public async Task GetAsync_OkWithUnmatchedParams()
        {
            // Arrange
            // N/A; empty service layer response will produce no results by default

            // Act
            ActionResult<MessageSizeResponse> controllerResponse = await this._controller
                .GetAsync(
                    10.1234,
                    -10.1234,
                    4,
                    DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(OkObjectResult));
            OkObjectResult castedResult = controllerResponse.Result as OkObjectResult;
            Assert.IsInstanceOfType(castedResult.Value, typeof(MessageSizeResponse));
            MessageSizeResponse responseResult = castedResult.Value as MessageSizeResponse;
            Assert.AreEqual(SizeController.NOT_FOUND_RESPONSE, responseResult.SizeOfQueryResponse);
        }
    }
}