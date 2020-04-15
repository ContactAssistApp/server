using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.API.v1.Controllers.MessageControllers;
using CovidSafe.DAL.Repositories;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CovidSafe.API.Tests.v1.Controllers.MessageControllers
{
    /// <summary>
    /// Unit Tests for the <see cref="ListController"/> class
    /// </summary>
    [TestClass]
    public class ListControllerTests
    {
        /// <summary>
        /// Test <see cref="HttpContext"/> instance
        /// </summary>
        private Mock<HttpContext> _context;
        /// <summary>
        /// Test <see cref="ListController"/> instance
        /// </summary>
        private ListController _controller;
        /// <summary>
        /// Mock <see cref="IMatchMessageRepository"/> instance
        /// </summary>
        private Mock<IMatchMessageRepository> _repo;
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
            this._repo = new Mock<IMatchMessageRepository>();

            // Configure service
            this._service = new MessageService(this._repo.Object);

            // Create HttpContext mock
            this._context = new Mock<HttpContext>();
            ActionContext actionContext = new ActionContext
            {
                HttpContext = this._context.Object,
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new ControllerActionDescriptor()
            };

            // Configure controller
            this._controller = new ListController(this._service);
            this._controller.ControllerContext = new ControllerContext(actionContext);
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
            Region requestedRegion = new Region
            {
                LatitudePrefix = 10.1234,
                LongitudePrefix = -10.1234,
                Precision = 4
            };
            int requestedTimestamp = 0;

            IEnumerable<MessageInfo> response = new List<MessageInfo>
            {
                new MessageInfo
                {
                    MessageId = "00000000-0000-0000-0000-0000000000",
                    MessageTimestamp = 0
                }
            };

            this._repo
                .Setup(s => s.GetLatestAsync(
                    It.IsAny<Region>(),
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
                    requestedTimestamp,
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
                .GetAsync(10.1234, -10.1234, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(OkObjectResult));
            OkObjectResult castedResult = controllerResponse.Result as OkObjectResult;
            Assert.IsInstanceOfType(castedResult.Value, typeof(MessageListResponse));
            MessageListResponse responseResult = castedResult.Value as MessageListResponse;
            Assert.AreEqual(responseResult.MessageInfoes.Count, 0);
        }
    }
}