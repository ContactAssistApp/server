using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.API.Controllers.MessageControllers;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CovidSafe.API.Tests.Controllers.MessageControllers
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
        /// Mock <see cref="IMessageService"/>
        /// </summary>
        private Mock<IMessageService> _service;

        /// <summary>
        /// Creates a new <see cref="ListControllerTests"/> instance
        /// </summary>
        public ListControllerTests()
        {
            // Configure service object
            this._service = new Mock<IMessageService>();

            // Create HttpContext mock
            this._context = new Mock<HttpContext>();
            ActionContext actionContext = new ActionContext
            {
                HttpContext = this._context.Object,
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new ControllerActionDescriptor()
            };

            // Configure controller
            this._controller = new ListController(this._service.Object);
            this._controller.ControllerContext = new ControllerContext(actionContext);
        }

        /// <summary>
        /// <see cref="ListController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestResult"/> with invalid timestamp
        /// </summary>
        [TestMethod]
        public async Task GetAsync_BadRequestWithInvalidTimestamp()
        {
            // Arrange
            // N/A

            // Act
            // Min Latitude is -90
            ActionResult<MessageListResponse> controllerResponse = await this._controller
                .GetAsync(10.1234, -10.1234, 4, -1, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestResult));
        }

        /// <summary>
        /// <see cref="ListController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestResult"/> with too high Latitude
        /// </summary>
        [TestMethod]
        public async Task GetAsync_BadRequestWithTooHighLatitude()
        {
            // Arrange
            // N/A

            // Act
            // Max Latitude is 90
            ActionResult<MessageListResponse> controllerResponse = await this._controller
                .GetAsync(91, -10.1234, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestResult));
        }

        /// <summary>
        /// <see cref="ListController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestResult"/> with too high Longitude
        /// </summary>
        [TestMethod]
        public async Task GetAsync_BadRequestWithTooHighLongitude()
        {
            // Arrange
            // N/A

            // Act
            // Max Longitude is 180
            ActionResult<MessageListResponse> controllerResponse = await this._controller
                .GetAsync(10.1234, 181, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestResult));
        }

        /// <summary>
        /// <see cref="ListController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestResult"/> with too high Latitude
        /// </summary>
        [TestMethod]
        public async Task GetAsync_BadRequestWithTooLowLatitude()
        {
            // Arrange
            // N/A

            // Act
            // Min Latitude is -90
            ActionResult<MessageListResponse> controllerResponse = await this._controller
                .GetAsync(-91, -10.1234, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestResult));
        }

        /// <summary>
        /// <see cref="ListController.GetAsync(double, double, int, long, CancellationToken)"/> 
        /// returns <see cref="BadRequestResult"/> with too low Longitude
        /// </summary>
        [TestMethod]
        public async Task GetAsync_BadRequestWithTooLowLongitude()
        {
            // Arrange
            // N/A

            // Act
            // Min Longitude is -180
            ActionResult<MessageListResponse> controllerResponse = await this._controller
                .GetAsync(10.1234, -181, 4, 0, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestResult));
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
            int requestedTimestamp = 0;

            IEnumerable<MessageInfo> response = new List<MessageInfo>
            {
                new MessageInfo
                {
                    MessageId = "00000000-0000-0000-0000-0000000000",
                    MessageTimestamp = 0
                }
            };

            this._service
                .Setup(s => s.GetLatestInfoAsync(
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
        /// <see cref="SizeController.GetAsync(double, double, int, long, CancellationToken)"/> 
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