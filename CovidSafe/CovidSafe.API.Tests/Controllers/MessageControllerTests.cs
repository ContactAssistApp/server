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
        /// <see cref="MessageController.PostAsync()"/> returns <see cref="OkResponse"/> 
        /// with matched parameters
        /// </summary>
        [TestMethod]
        public void PostAsync_OkResponseWithValidData()
        {
            // Arrange
            string[] ids = new string[]
            {
                "00000000-0000-0000-0000-000000000000",
                "00000000-0000-0000-0000-000000000001"
            };
            MatchMessage result1 = new MatchMessage();
            MatchMessage result2 = new MatchMessage();
            IEnumerable<MatchMessage> response = new List<MatchMessage>
            {
                result1,
                result2
            };

            this._service
                .Setup(s => s.GetByIdsAsync(ids, CancellationToken.None))
                .Returns(Task.FromResult(response));

            MessageRequest request = new MessageRequest();
            request.RequestedQueries.Add(new MessageInfo
            {
                MessageId = ids[0],
                MessageTimestamp = 0
            });
            request.RequestedQueries.Add(new MessageInfo
            {
                MessageId = ids[1],
                MessageTimestamp = 0
            });

            // Act
            ActionResult<IEnumerable<MatchMessage>> controllerResponse = this._controller
                .PostAsync(request, CancellationToken.None)
                .Result;

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse.Value, typeof(IEnumerable<MatchMessage>));
            Assert.AreEqual(response.Count(), controllerResponse.Value.Count());
        }

        /// <summary>
        /// <see cref="MessageController.PostAsync()"/> returns <see cref="NotFoundResponse"/> 
        /// with unmatched parameters
        /// </summary>
        [TestMethod]
        public void PostAsync_NotFoundResponseWithInvalidParams()
        {
        }
    }
}