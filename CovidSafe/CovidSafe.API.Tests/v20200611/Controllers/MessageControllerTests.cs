using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using CovidSafe.API.v20200611.Controllers;
using CovidSafe.API.v20200611.Protos;
using CovidSafe.DAL.Repositories;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CovidSafe.API.v20200611.Tests.Controllers
{
    /// <summary>
    /// Unit Tests for the <see cref="MessageController"/> class
    /// </summary>
    [TestClass]
    public class MessageControllerTests
    {
        /// <summary>
        /// Test <see cref="MessageController"/> instance
        /// </summary>
        private MessageController _controller;
        /// <summary>
        /// Mock <see cref="IMessageContainerRepository"/>
        /// </summary>
        private Mock<IMessageContainerRepository> _repo;
        /// <summary>
        /// <see cref="MessageService"/> instance
        /// </summary>
        private MessageService _service;
        
        /// <summary>
        /// Creates a new <see cref="MessageControllerTests"/> instance
        /// </summary>
        public MessageControllerTests()
        {
            // Configure repository
            this._repo = new Mock<IMessageContainerRepository>();

            // Configure Service instance
            this._service = new MessageService(this._repo.Object);

            // Create AutoMapper instance
            MapperConfiguration mapperConfig = new MapperConfiguration(
                opts => opts.AddProfile<MappingProfiles>()
            );
            IMapper mapper = mapperConfig.CreateMapper();

            // Configure controller
            this._controller = new MessageController(mapper, this._service);
            this._controller.ControllerContext = new ControllerContext();
            this._controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        /// <summary>
        /// <see cref="MessageController.HeadAsync(CancellationToken)"/> always 
        /// returns a <see cref="OkResult"/>
        /// </summary>
        [TestMethod]
        public void HeadAsync_OkResultAlways()
        {
            // Arrange
            // N/A

            // Act
            ActionResult controllerResponse = this._controller
                .HeadAsync(CancellationToken.None)
                .Result;

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(OkResult));
        }

        /// <summary>
        /// <see cref="MessageController.PostAsync()"/> returns <see cref="BadRequestObjectResult"/> 
        /// with invalid parameters
        /// </summary>
        [TestMethod]
        public async Task PostAsync_BadRequestObjectWithInvalidParams()
        {
            // Arrange
            MessageRequest request = new MessageRequest();
            request.RequestedQueries.Add(new MessageInfo
            {
                MessageId = "Not a GUID!", // Invalid format
                MessageTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });

            // Act
            ActionResult<MessageResponse> controllerResponse = await this._controller
                .PostAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="MessageController.PostAsync()"/> returns <see cref="BadRequestObjectResult"/> 
        /// with null parameters
        /// </summary>
        [TestMethod]
        public async Task PostAsync_BadRequestWithNullParams()
        {
            // Arrange
            MessageRequest request = new MessageRequest(); // Empty request

            // Act
            ActionResult<MessageResponse> controllerResponse = await this._controller
                .PostAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestResult));
        }

        /// <summary>
        /// <see cref="MessageController.PostAsync()"/> returns empty 
        /// <see cref="OkObjectResult"/> with unmatched parameters
        /// </summary>
        [TestMethod]
        public async Task PostAsync_EmptyOkWithUnmatchedParams()
        {
            // Arrange
            MessageRequest request = new MessageRequest();
            request.RequestedQueries.Add(new MessageInfo
            {
                MessageId = "00000000-0000-0000-0000-000000000002",
                MessageTimestamp = 0
            });
            request.RequestedQueries.Add(new MessageInfo
            {
                MessageId = "00000000-0000-0000-0000-000000000003",
                MessageTimestamp = 0
            });

            // Act
            ActionResult<MessageResponse> controllerResponse = await this._controller
                .PostAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(OkObjectResult));
            OkObjectResult castedResult = controllerResponse.Result as OkObjectResult;
            Assert.IsInstanceOfType(castedResult.Value, typeof(MessageResponse));
            MessageResponse listResult = castedResult.Value as MessageResponse;
            Assert.AreEqual(0, listResult.NarrowcastMessages.Count());
        }

        /// <summary>
        /// <see cref="MessageController.PostAsync()"/> returns <see cref="OkObjectResult"/> 
        /// with matched parameters
        /// </summary>
        [TestMethod]
        public async Task PostAsync_OkWithMatchedParameters()
        {
            // Arrange
            IEnumerable<string> ids = new string[]
            {
                "00000000-0000-0000-0000-000000000001",
                "00000000-0000-0000-0000-000000000002"
            };
            MessageContainer result1 = new MessageContainer();
            result1.Narrowcasts.Add(new Entities.Messages.NarrowcastMessage
            {
                UserMessage = "This is one message."
            });
            MessageContainer result2 = new MessageContainer();
            result2.Narrowcasts.Add(new Entities.Messages.NarrowcastMessage
            {
                UserMessage = "This is another."
            });
            IEnumerable<MessageContainer> toReturn = new List<MessageContainer>
            {
                result1,
                result2
            };

            this._repo
                .Setup(s => s.GetRangeAsync(ids, CancellationToken.None))
                .Returns(Task.FromResult(toReturn));

            MessageRequest request = new MessageRequest();
            request.RequestedQueries.Add(new MessageInfo
            {
                MessageId = ids.ElementAt(0),
                MessageTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });
            request.RequestedQueries.Add(new MessageInfo
            {
                MessageId = ids.ElementAt(1),
                MessageTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });

            // Act
            ActionResult<MessageResponse> controllerResponse = await this._controller
                .PostAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(OkObjectResult));
            OkObjectResult castedResult = controllerResponse.Result as OkObjectResult;
            Assert.IsInstanceOfType(castedResult.Value, typeof(MessageResponse));
            MessageResponse listResult = castedResult.Value as MessageResponse;
            Assert.AreEqual(toReturn.Count(), listResult.NarrowcastMessages.Count);
        }

        /// <summary>
        /// <see cref="MessageController.PostAsync()"/> returns <see cref="OkObjectResult"/> 
        /// with partially matched parameters
        /// </summary>
        [TestMethod]
        public async Task PostAsync_OkWithPartiallyMatchedParameters()
        {
            // Arrange
            IEnumerable<string> ids = new string[]
            {
                "00000000-0000-0000-0000-000000000001",
                "00000000-0000-0000-0000-000000000002"
            };
            MessageContainer result1 = new MessageContainer();
            result1.Narrowcasts.Add(new Entities.Messages.NarrowcastMessage
            {
                UserMessage = "This is a message"
            });
            IEnumerable<MessageContainer> toReturn = new List<MessageContainer>
            {
                result1
            };

            this._repo
                .Setup(s => s.GetRangeAsync(ids, CancellationToken.None))
                .Returns(Task.FromResult(toReturn));

            MessageRequest request = new MessageRequest();
            request.RequestedQueries.Add(new MessageInfo
            {
                MessageId = ids.ElementAt(0),
                MessageTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });
            request.RequestedQueries.Add(new MessageInfo
            {
                MessageId = ids.ElementAt(1),
                MessageTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });

            // Act
            ActionResult<MessageResponse> controllerResponse = await this._controller
                .PostAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(OkObjectResult));
            OkObjectResult castedResult = controllerResponse.Result as OkObjectResult;
            Assert.IsInstanceOfType(castedResult.Value, typeof(MessageResponse));
            MessageResponse listResult = castedResult.Value as MessageResponse;
            Assert.AreEqual(1, listResult.NarrowcastMessages.Count());
        }
    }
}