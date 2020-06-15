using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using CovidSafe.API.v20200505.Controllers.MessageControllers;
using CovidSafe.API.v20200505.Protos;
using CovidSafe.DAL.Repositories;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CovidSafe.API.v20200505.Tests.Controllers.MessageControllers
{
    /// <summary>
    /// Unit Tests for the <see cref="ListController"/> class
    /// </summary>
    [TestClass]
    public class ListControllerTests
    {
        /// <summary>
        /// Test <see cref="ListController"/> instance
        /// </summary>
        private ListController _controller;
        /// <summary>
        /// Mock <see cref="IMessageContainerRepository"/> instance
        /// </summary>
        private Mock<IMessageContainerRepository> _repo;
        /// <summary>
        /// <see cref="MessageService"/> instance
        /// </summary>
        private MessageService _service;

        /// <summary>
        /// Creates a new <see cref="ListControllerTests"/> instance
        /// </summary>
        public ListControllerTests()
        {
            // Configure repo mock
            this._repo = new Mock<IMessageContainerRepository>();

            // Configure service
            this._service = new MessageService(this._repo.Object);

            // Create AutoMapper instance
            MapperConfiguration mapperConfig = new MapperConfiguration(
                opts => opts.AddProfile<MappingProfiles>()
            );
            IMapper mapper = mapperConfig.CreateMapper();

            // Configure controller
            this._controller = new ListController(mapper, this._service);
            this._controller.ControllerContext = new ControllerContext();
            this._controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        /// <summary>
        /// <see cref="ListController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with invalid timestamp
        /// </summary>
        [TestMethod]
        public async Task GetAsync_BadRequestObjectWithInvalidTimestamp()
        {
            // Arrange
            // N/A

            // Act
            // Min Latitude is -90
            ActionResult<MessageListResponse> controllerResponse = await this._controller
                .GetAsync(10.1234, -10.1234, 4, -1, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="ListController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with too high Latitude
        /// </summary>
        [TestMethod]
        public async Task GetAsync_BadRequestObjectWithTooHighLatitude()
        {
            // Arrange
            // N/A

            // Act
            // Max Latitude is 90
            ActionResult<MessageListResponse> controllerResponse = await this._controller
                .GetAsync(91, -10.1234, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="ListController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with too high Longitude
        /// </summary>
        [TestMethod]
        public async Task GetAsync_BadRequestObjectWithTooHighLongitude()
        {
            // Arrange
            // N/A

            // Act
            // Max Longitude is 180
            ActionResult<MessageListResponse> controllerResponse = await this._controller
                .GetAsync(10.1234, 181, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="ListController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with too high Latitude
        /// </summary>
        [TestMethod]
        public async Task GetAsync_BadRequestObjectWithTooLowLatitude()
        {
            // Arrange
            // N/A

            // Act
            // Min Latitude is -90
            ActionResult<MessageListResponse> controllerResponse = await this._controller
                .GetAsync(-91, -10.1234, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="ListController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with too low Longitude
        /// </summary>
        [TestMethod]
        public async Task GetAsync_BadRequestObjectWithTooLowLongitude()
        {
            // Arrange
            // N/A

            // Act
            // Min Longitude is -180
            ActionResult<MessageListResponse> controllerResponse = await this._controller
                .GetAsync(10.1234, -181, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="ListController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="OkObjectResult"/> with matched parameters
        /// </summary>
        [TestMethod]
        public async Task GetAsync_OkWithMatchedParams()
        {
            // Arrange
            v20200505.Protos.Region requestedRegion = new Region
            {
                LatitudePrefix = 10.1234,
                LongitudePrefix = -10.1234,
                Precision = 4
            };

            IEnumerable<MessageContainerMetadata> response = new List<MessageContainerMetadata>
            {
                new MessageContainerMetadata
                {
                    Id = "00000000-0000-0000-0000-0000000001",
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                }
            };

            this._repo
                .Setup(s => s.GetLatestAsync(
                    It.IsAny<Entities.Geospatial.Region>(),
                    It.IsAny<long>(),
                    CancellationToken.None
                ))
                .Returns(Task.FromResult(response));

            // Act
            ActionResult<MessageListResponse> controllerResponse = await this._controller
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
            Assert.IsInstanceOfType(castedResult.Value, typeof(MessageListResponse));
            MessageListResponse responseResult = castedResult.Value as MessageListResponse;
            Assert.AreEqual(responseResult.MessageInfoes.Count(), response.Count());
        }

        /// <summary>
        /// <see cref="ListController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="OkObjectResult"/> with unmatched parameters
        /// </summary>
        [TestMethod]
        public async Task GetAsync_EmptyOkWithUnmatchedParams()
        {
            // Arrange
            // N/A; empty service layer response will produce no results by default

            // Act
            ActionResult<MessageListResponse> controllerResponse = await this._controller
                .GetAsync(
                    10.1234,
                    -10.1234,
                    4,
                    DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    CancellationToken.None
                );

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(OkObjectResult));
            OkObjectResult castedResult = controllerResponse.Result as OkObjectResult;
            Assert.IsInstanceOfType(castedResult.Value, typeof(MessageListResponse));
            MessageListResponse responseResult = castedResult.Value as MessageListResponse;
            Assert.AreEqual(responseResult.MessageInfoes.Count, 0);
        }

        /// <summary>
        /// <see cref="ListController.HeadAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with invalid timestamp
        /// </summary>
        [TestMethod]
        public async Task HeadAsync_BadRequestObjectWithInvalidTimestamp()
        {
            // Arrange
            // N/A

            // Act
            // Min Latitude is -90
            ActionResult controllerResponse = await this._controller
                .HeadAsync(10.1234, -10.1234, 4, -1, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="ListController.HeadAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with too high Latitude
        /// </summary>
        [TestMethod]
        public async Task HeadAsync_BadRequestObjectWithTooHighLatitude()
        {
            // Arrange
            // N/A

            // Act
            // Max Latitude is 90
            ActionResult controllerResponse = await this._controller
                .HeadAsync(91, -10.1234, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="ListController.HeadAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with too high Longitude
        /// </summary>
        [TestMethod]
        public async Task HeadAsync_BadRequestObjectWithTooHighLongitude()
        {
            // Arrange
            // N/A

            // Act
            // Max Longitude is 180
            ActionResult controllerResponse = await this._controller
                .HeadAsync(10.1234, 181, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="ListController.HeadAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with too high Latitude
        /// </summary>
        [TestMethod]
        public async Task HeadAsync_BadRequestObjectWithTooLowLatitude()
        {
            // Arrange
            // N/A

            // Act
            // Min Latitude is -90
            ActionResult controllerResponse = await this._controller
                .HeadAsync(-91, -10.1234, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="ListController.HeadAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with too low Longitude
        /// </summary>
        [TestMethod]
        public async Task HeadAsync_BadRequestObjectWithTooLowLongitude()
        {
            // Arrange
            // N/A

            // Act
            // Min Longitude is -180
            ActionResult controllerResponse = await this._controller
                .HeadAsync(10.1234, -181, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="ListController.HeadAsync(double, double, int, long, CancellationToken)"/> 
        /// returns Content-Length header of appropriate size when parameters match
        /// </summary>
        [TestMethod]
        public async Task HeadAsync_ContentLengthHeaderSetWithValidParams()
        {
            // Arrange
            long repoResponse = 1024;
            this._repo
                .Setup(
                    r => r.GetLatestRegionSizeAsync(
                        It.IsAny<Entities.Geospatial.Region>(),
                        It.IsAny<long>(),
                        CancellationToken.None
                    )
                )
                .Returns(Task.FromResult(repoResponse));

            // Act
            ActionResult controllerResponse = await this._controller
                .HeadAsync(
                    10.1234,
                    -10.1234,
                    4,
                    DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    CancellationToken.None
                );

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsNotNull(this._controller.HttpContext.Response.ContentLength);
            Assert.AreEqual(repoResponse, this._controller.HttpContext.Response.ContentLength);
        }

        /// <summary>
        /// <see cref="ListController.HeadAsync(double, double, int, long, CancellationToken)"/> 
        /// returns Content-Length header of '0' when parameters do not return results
        /// </summary>
        [TestMethod]
        public async Task HeadAsync_ContentLengthHeaderZeroWithInvalidParams()
        {
            // Arrange
            long repoResponse = 0;

            // Act
            ActionResult controllerResponse = await this._controller
                .HeadAsync(
                    10.1234,
                    -10.1234,
                    4,
                    DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    CancellationToken.None
                );

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsNotNull(this._controller.HttpContext.Response.ContentLength);
            Assert.AreEqual(repoResponse, this._controller.HttpContext.Response.ContentLength);
        }
    }
}