using System.Collections.Generic;
using System.Threading;

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
    /// Unit tests for the <see cref="SeedReportController"/> class
    /// </summary>
    [TestClass]
    public class SeedReportControllerTests
    {
        /// <summary>
        /// Test <see cref="HttpContext"/> instance
        /// </summary>
        private Mock<HttpContext> _context;
        /// <summary>
        /// Test <see cref="SeedReportController"/> instance
        /// </summary>
        private SeedReportController _controller;
        /// <summary>
        /// Mock <see cref="IMessageService"/>
        /// </summary>
        private Mock<IMessageService> _service;

        /// <summary>
        /// Creates a new <see cref="AreaReportControllerTests"/> instance
        /// </summary>
        public SeedReportControllerTests()
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
            this._controller = new SeedReportController(this._service.Object);
            this._controller.ControllerContext = new ControllerContext(actionContext);
        }

        /// <summary>
        /// <see cref="SeedReportController.PutAsync(SelfReportRequest, CancellationToken)"/> 
        /// returns <see cref="BadRequestResult"/> with invalid Client Timestamp provided 
        /// with request
        /// </summary>
        [TestMethod]
        public void PutAsync_BadResultRequestWithInvalidClientTimestamp()
        {
            // Arrange
            SelfReportRequest requestObj = new SelfReportRequest
            {
                ClientTimestamp = -1,
                Region = new Region
                {
                    LatitudePrefix = 10.1234,
                    LongitudePrefix = 10.1234,
                    Precision = 4
                }
            };
            requestObj.Seeds.Add(new BlueToothSeed
            {
                EstimatedSkew = 1,
                Seed = "00000000-0000-0000-0000-000000000000",
                SequenceEndTime = 1,
                SequenceStartTime = 0
            });

            // Act
            ActionResult controllerResponse = this._controller
                .PutAsync(requestObj, CancellationToken.None)
                .Result;

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestResult));
        }

        /// <summary>
        /// <see cref="SeedReportController.PutAsync(SelfReportRequest, CancellationToken)"/> 
        /// returns <see cref="BadRequestResult"/> with invalid <see cref="BlueToothSeed"/> provided  
        /// with request
        /// </summary>
        [TestMethod]
        public void PutAsync_BadResultRequestWithInvalidSeed()
        {
            // Arrange
            SelfReportRequest requestObj = new SelfReportRequest
            {
                ClientTimestamp = 0,
                Region = new Region
                {
                    LatitudePrefix = 10.1234,
                    LongitudePrefix = 10.1234,
                    Precision = 4
                }
            };
            requestObj.Seeds.Add(new BlueToothSeed
            {
                EstimatedSkew = 1,
                Seed = "Invalid seed format!",
                SequenceEndTime = 1,
                SequenceStartTime = 0
            });

            // Act
            ActionResult controllerResponse = this._controller
                .PutAsync(requestObj, CancellationToken.None)
                .Result;

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="SeedReportController.PutAsync(SelfReportRequest, CancellationToken)"/> 
        /// returns <see cref="BadRequestResult"/> with invalid <see cref="Region"/> provided  
        /// with request
        /// </summary>
        [TestMethod]
        public void PutAsync_BadResultRequestWithNoRegion()
        {
            // Arrange
            SelfReportRequest requestObj = new SelfReportRequest
            {
                ClientTimestamp = 0
            };
            requestObj.Seeds.Add(new BlueToothSeed
            {
                EstimatedSkew = 1,
                Seed = "00000000-0000-0000-0000-000000000000",
                SequenceEndTime = 1,
                SequenceStartTime = 0
            });

            // Act
            ActionResult controllerResponse = this._controller
                .PutAsync(requestObj, CancellationToken.None)
                .Result;

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestResult));
        }

        /// <summary>
        /// <see cref="SeedReportController.PutAsync(SelfReportRequest, CancellationToken)"/> 
        /// returns <see cref="BadRequestResult"/> when no <see cref="Seed"/> objects are provided 
        /// with request
        /// </summary>
        [TestMethod]
        public void PutAsync_BadResultRequestWithNoSeeds()
        {
            // Arrange
            SelfReportRequest requestObj = new SelfReportRequest
            {
                ClientTimestamp = 0,
                Region = new Region
                {
                    LatitudePrefix = 10.1234,
                    LongitudePrefix = 10.1234,
                    Precision = 4
                }
            };

            // Act
            ActionResult controllerResponse = this._controller
                .PutAsync(requestObj, CancellationToken.None)
                .Result;

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestResult));
        }

        /// <summary>
        /// <see cref="SeedReportController.PutAsync(SelfReportRequest, CancellationToken)"/> 
        /// returns <see cref="OkResult"/> with valid input data
        /// </summary>
        [TestMethod]
        public void PutAsync_OkResultWithValidInputs()
        {
            // Arrange
            SelfReportRequest requestObj = new SelfReportRequest
            {
                ClientTimestamp = 0,
                Region = new Region
                {
                    LatitudePrefix = 10.1234,
                    LongitudePrefix = 10.1234,
                    Precision = 4
                }
            };
            requestObj.Seeds.Add(new BlueToothSeed
            {
                EstimatedSkew = 10,
                SequenceEndTime = 1,
                SequenceStartTime = 0,
                Seed = "00000000-0000-0000-0000-000000000000"
            });

            // Act
            ActionResult controllerResponse = this._controller
                .PutAsync(requestObj, CancellationToken.None)
                .Result;

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(OkResult));
        }
    }
}
