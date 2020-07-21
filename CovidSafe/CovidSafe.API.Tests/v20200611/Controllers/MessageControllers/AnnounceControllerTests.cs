using System;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using CovidSafe.API.v20200611.Controllers.MessageControllers;
using CovidSafe.API.v20200611.Protos;
using CovidSafe.DAL.Repositories;
using CovidSafe.DAL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CovidSafe.API.v20200611.Tests.Controllers.MessageControllers
{
    /// <summary>
    /// Unit tests for the <see cref="AnnounceController"/> class
    /// </summary>
    [TestClass]
    public class AnnounceControllerTests
    {
        /// <summary>
        /// Test <see cref="AnnounceController"/> instance
        /// </summary>
        private AnnounceController _controller;
        /// <summary>
        /// Mock <see cref="IMessageContainerRepository"/> instance
        /// </summary>
        private Mock<IMessageContainerRepository> _repo;
        /// <summary>
        /// <see cref="MessageService"/> instance
        /// </summary>
        private MessageService _service;

        /// <summary>
        /// Creates a new <see cref="AnnounceControllerTests"/> instance
        /// </summary>
        public AnnounceControllerTests()
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
            this._controller = new AnnounceController(mapper, this._service);
            this._controller.ControllerContext = new ControllerContext();
            this._controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        /// <summary>
        /// <see cref="AnnounceController.DeleteAsync(string, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> when no <see cref="Area"/> objects are provided 
        /// with request
        /// </summary>
        [TestMethod]
        public async Task DeleteAsync_BadRequestObjectWithInvalidMessageId()
        {
            // Arrange
            string invalidId = "this is an invalid ID.";

            // Act
            ActionResult controllerResponse = await this._controller
                .DeleteAsync(invalidId, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="AnnounceController.DeleteAsync(string, CancellationToken)"/> 
        /// returns <see cref="NotFoundResult"/> when no <see cref="NarrowcastMessage"/> 
        /// objects are matched by the provided 'messageId' parameter
        /// </summary>
        [TestMethod]
        public async Task DeleteAsync_NotFoundWithInvalidMessageId()
        {
            // Arrange
            string unmatchedId = "00000000-0000-0000-0000-000000000001";
            this._repo
                .Setup(r => r.DeleteAsync(It.IsAny<string>(), CancellationToken.None))
                .Returns(Task.FromResult(true));

            // Act
            ActionResult controllerResponse = await this._controller
                .DeleteAsync(unmatchedId, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(NotFoundResult));
        }

        /// <summary>
        /// <see cref="AnnounceController.DeleteAsync(string, CancellationToken)"/> 
        /// returns <see cref="OkResult"/> with valid input data
        /// </summary>
        [TestMethod]
        public async Task DeleteAsync_OkWithValidInputs()
        {
            // Arrange
            string validId = "00000000-0000-0000-0000-000000000001";
            this._repo
                .Setup(r => r.DeleteAsync(validId, CancellationToken.None))
                .Returns(Task.FromResult(true));

            // Act
            ActionResult controllerResponse = await this._controller
                .DeleteAsync(validId, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(OkResult));
        }

        /// <summary>
        /// <see cref="AnnounceController.PutAsync(NarrowcastMessage, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> when no <see cref="Area"/> objects are provided 
        /// with request
        /// </summary>
        [TestMethod]
        public async Task PutAsync_BadRequestObjectWithNoAreas()
        {
            // Arrange
            NarrowcastMessage requestObj = new NarrowcastMessage
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
        /// <see cref="AnnounceController.PutAsync(NarrowcastMessage, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> when no user message is specified
        /// </summary>
        [TestMethod]
        public async Task PutAsync_BadRequestWithNoUserMessage()
        {
            // Arrange
            NarrowcastMessage requestObj = new NarrowcastMessage();
            requestObj.Area = new Area
            {
                BeginTime = 0,
                EndTime = 1,
                Location = new Location
                {
                    Latitude = 10.1234,
                    Longitude = 10.1234
                },
                RadiusMeters = 100
            };

            // Act
            ActionResult controllerResponse = await this._controller
                .PutAsync(requestObj, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="AnnounceController.PutAsync(NarrowcastMessage, CancellationToken)"/> 
        /// returns <see cref="OkResult"/> with valid input data
        /// </summary>
        [TestMethod]
        public async Task PutAsync_OkWithValidInputs()
        {
            // Arrange
            NarrowcastMessage requestObj = new NarrowcastMessage
            {
                UserMessage = "User message content"
            };
            requestObj.Area = new Area
            {
                BeginTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                EndTime = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeMilliseconds(),
                Location = new Location
                {
                    Latitude = 10.1234,
                    Longitude = 10.1234
                },
                RadiusMeters = 100
            };

            // Act
            ActionResult controllerResponse = await this._controller
                .PutAsync(requestObj, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(OkResult));
        }
    }
}
