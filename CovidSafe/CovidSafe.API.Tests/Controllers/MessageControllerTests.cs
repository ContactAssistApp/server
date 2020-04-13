using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.API.Controllers;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CovidSafe.API.Tests.Controllers
{
    /// <summary>
    /// Unit Tests for the <see cref="MessageController"/> class
    /// </summary>
    [TestClass]
    public class MessageControllerTests
    {
        /// <summary>
        /// Test <see cref="HttpContext"/> instance
        /// </summary>
        private Mock<HttpContext> _context;
        /// <summary>
        /// Test <see cref="MessageController"/> instance
        /// </summary>
        private MessageController _controller;
        /// <summary>
        /// Mock <see cref="IMessageService"/>
        /// </summary>
        private Mock<IMessageService> _service;
        
        /// <summary>
        /// Creates a new <see cref="MessageControllerTests"/> instance
        /// </summary>
        public MessageControllerTests()
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
            this._controller = new MessageController(this._service.Object);
            this._controller.ControllerContext = new ControllerContext(actionContext);
        }

        /// <summary>
        /// <see cref="MessageController.HeadAsync(CancellationToken)"/> always 
        /// returns a <see cref="OkResult"/>
        /// </summary>
        [TestMethod]
        public void HeadAsync_AlwaysReturnsOkResult()
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
        /// <see cref="MessageController.PostAsync()"/> returns <see cref="BadRequestResult"/> 
        /// with invalid parameters
        /// </summary>
        [TestMethod]
        public void PostAsync_BadRequestResultWithInvalidParams()
        {
            // Arrange
            MessageRequest request = new MessageRequest();
            request.RequestedQueries.Add(new MessageInfo
            {
                MessageId = "Not a GUID!", // Invalid format
                MessageTimestamp = 0
            });

            // Act
            ActionResult<IEnumerable<MatchMessage>> controllerResponse = this._controller
                .PostAsync(request, CancellationToken.None)
                .Result;

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestResult));
        }

        /// <summary>
        /// <see cref="MessageController.PostAsync()"/> returns <see cref="BadRequestResult"/> 
        /// with null parameters
        /// </summary>
        [TestMethod]
        public void PostAsync_BadRequestResultWithNullParams()
        {
            // Arrange
            MessageRequest request = new MessageRequest(); // Empty request

            // Act
            ActionResult<IEnumerable<MatchMessage>> controllerResponse = this._controller
                .PostAsync(request, CancellationToken.None)
                .Result;

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(BadRequestResult));
        }

        /// <summary>
        /// <see cref="MessageController.PostAsync()"/> returns empty 
        /// <see cref="OkObjectResult"/> with unmatched parameters
        /// </summary>
        [TestMethod]
        public void PostAsync_EmptyOkObjectResultWithUnmatchedParams()
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
            ActionResult<IEnumerable<MatchMessage>> controllerResponse = this._controller
                .PostAsync(request, CancellationToken.None)
                .Result;

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(OkObjectResult));
            OkObjectResult castedResult = controllerResponse.Result as OkObjectResult;
            Assert.IsInstanceOfType(castedResult.Value, typeof(IEnumerable<MatchMessage>));
            IEnumerable<MatchMessage> listResult = castedResult.Value as IEnumerable<MatchMessage>;
            Assert.AreEqual(0, listResult.Count());
        }

        /// <summary>
        /// <see cref="MessageController.PostAsync()"/> returns <see cref="OkObjectResult"/> 
        /// with matched parameters
        /// </summary>
        [TestMethod]
        public void PostAsync_OkObjectResultWithMatchedParameters()
        {
            // Arrange
            IEnumerable<string> ids = new string[]
            {
                "00000000-0000-0000-0000-000000000000",
                "00000000-0000-0000-0000-000000000001"
            };
            MatchMessage result1 = new MatchMessage();
            MatchMessage result2 = new MatchMessage();
            IEnumerable<MatchMessage> toReturn = new List<MatchMessage>
            {
                result1,
                result2
            };

            this._service
                .Setup(s => s.GetByIdsAsync(ids, CancellationToken.None))
                .Returns(Task.FromResult(toReturn));

            MessageRequest request = new MessageRequest();
            request.RequestedQueries.Add(new MessageInfo
            {
                MessageId = ids.ElementAt(0),
                MessageTimestamp = 0
            });
            request.RequestedQueries.Add(new MessageInfo
            {
                MessageId = ids.ElementAt(1),
                MessageTimestamp = 0
            });

            // Act
            ActionResult<IEnumerable<MatchMessage>> controllerResponse = this._controller
                .PostAsync(request, CancellationToken.None)
                .Result;

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(OkObjectResult));
            OkObjectResult castedResult = controllerResponse.Result as OkObjectResult;
            Assert.IsInstanceOfType(castedResult.Value, typeof(List<MatchMessage>));
            List<MatchMessage> listResult = castedResult.Value as List<MatchMessage>;
            Assert.AreEqual(toReturn.Count(), listResult.Count());
        }

        /// <summary>
        /// <see cref="MessageController.PostAsync()"/> returns <see cref="OkObjectResult"/> 
        /// with partially matched parameters
        /// </summary>
        [TestMethod]
        public void PostAsync_OkObjectResultWithPartiallyMatchedParameters()
        {
            // Arrange
            IEnumerable<string> ids = new string[]
            {
                "00000000-0000-0000-0000-000000000000",
                "00000000-0000-0000-0000-000000000001"
            };
            MatchMessage result1 = new MatchMessage();
            IEnumerable<MatchMessage> toReturn = new List<MatchMessage>
            {
                result1
            };

            this._service
                .Setup(s => s.GetByIdsAsync(ids, CancellationToken.None))
                .Returns(Task.FromResult(toReturn));

            MessageRequest request = new MessageRequest();
            request.RequestedQueries.Add(new MessageInfo
            {
                MessageId = ids.ElementAt(0),
                MessageTimestamp = 0
            });
            request.RequestedQueries.Add(new MessageInfo
            {
                MessageId = ids.ElementAt(1),
                MessageTimestamp = 0
            });

            // Act
            ActionResult<IEnumerable<MatchMessage>> controllerResponse = this._controller
                .PostAsync(request, CancellationToken.None)
                .Result;

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Result, typeof(OkObjectResult));
            OkObjectResult castedResult = controllerResponse.Result as OkObjectResult;
            Assert.IsInstanceOfType(castedResult.Value, typeof(List<MatchMessage>));
            List<MatchMessage> listResult = castedResult.Value as List<MatchMessage>;
            Assert.AreEqual(1, listResult.Count());
        }
    }
}