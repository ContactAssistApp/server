using System.Threading;
using System.Threading.Tasks;

using CovidSafe.API.Controllers.MessageControllers;
using CovidSafe.DAL.Repositories;
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
    /// Unit tests for the <see cref="AreaReportController"/> class
    /// </summary>
    [TestClass]
    public class AreaReportControllerTests
    {
        /// <summary>
        /// Test <see cref="ListController"/> instance
        /// </summary>
        private AreaReportController _controller;
        /// <summary>
        /// Mock <see cref="IMatchMessageRepository"/> instance
        /// </summary>
        private Mock<IMatchMessageRepository> _repo;
        /// <summary>
        /// <see cref="MessageService"/> instance
        /// </summary>
        private MessageService _service;

        /// <summary>
        /// Creates a new <see cref="AreaReportControllerTests"/> instance
        /// </summary>
        public AreaReportControllerTests()
        {
            // Configure repo mock
            this._repo = new Mock<IMatchMessageRepository>();

            // Configure service
            this._service = new MessageService(this._repo.Object);

            // Configure controller
            this._controller = new AreaReportController(this._service);
            this._controller.ControllerContext = new ControllerContext();
            this._controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        /// <summary>
        /// <see cref="AreaReportController.PutAsync(AreaMatch, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> when no <see cref="Area"/> objects are provided 
        /// with request
        /// </summary>
        [TestMethod]
        public async Task PutAsync_BadRequestObjectWithNoAreas()
        {
            // Arrange
            AreaMatch requestObj = new AreaMatch
            {
                UserMessage = "This is a message"
            };

            // Act
            ActionResult controllerResponse = await this._controller
                .PutAsync(requestObj, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="AreaReportController.PutAsync(AreaMatch, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> when no user message is specified
        /// </summary>
        [TestMethod]
        public async Task PutAsync_BadRequestWithNoUserMessage()
        {
            // Arrange
            AreaMatch requestObj = new AreaMatch();
            requestObj.Areas.Add(new Area
            {
                BeginTime = 0,
                EndTime = 1,
                Location = new Location
                {
                    Latitude = 10.1234,
                    Longitude = 10.1234
                },
                RadiusMeters = 100
            });

            // Act
            ActionResult controllerResponse = await this._controller
                .PutAsync(requestObj, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="AreaReportController.PutAsync(AreaMatch, CancellationToken)"/> 
        /// returns <see cref="OkResult"/> with valid input data
        /// </summary>
        [TestMethod]
        public async Task PutAsync_OkWithValidInputs()
        {
            // Arrange
            AreaMatch requestObj = new AreaMatch
            {
                UserMessage = "User message content"
            };
            requestObj.Areas.Add(new Area
            {
                BeginTime = 0,
                EndTime = 1,
                Location = new Location
                {
                    Latitude = 10.1234,
                    Longitude = 10.1234
                },
                RadiusMeters = 100
            });

            // Act
            ActionResult controllerResponse = await this._controller
                .PutAsync(requestObj, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(OkResult));
        }
    }
}
