using System.Threading;
using System.Threading.Tasks;

using CovidSafe.API.Controllers.MessageControllers;
using CovidSafe.DAL.Repositories;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CovidSafe.API.Tests.v1.Controllers.MessageControllers
{
    /// <summary>
    /// Unit tests for the <see cref="SeedReportController"/> class
    /// </summary>
    [TestClass]
    public class SeedReportControllerTests
    {
        /// <summary>
        /// Test <see cref="SeedReportController"/> instance
        /// </summary>
        private SeedReportController _controller;
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
        public SeedReportControllerTests()
        {
            // Configure repo mock
            this._repo = new Mock<IMatchMessageRepository>();

            // Configure service
            this._service = new MessageService(this._repo.Object);

            // Configure controller
            this._controller = new SeedReportController(this._service);
            this._controller.ControllerContext = new ControllerContext();
            this._controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        /// <summary>
        /// <see cref="SeedReportController.PutAsync(SelfReportRequest, CancellationToken)"/> 
        /// returns <see cref="BadRequestResult"/> with invalid <see cref="BlueToothSeed"/> provided  
        /// with request
        /// </summary>
        [TestMethod]
        public async Task PutAsync_BadRequestObjectWithInvalidSeed()
        {
            // Arrange
            SelfReportRequest requestObj = new SelfReportRequest
            {
                Region = new Region
                {
                    LatitudePrefix = 10.1234,
                    LongitudePrefix = 10.1234,
                    Precision = 4
                }
            };
            requestObj.Seeds.Add(new BlueToothSeed
            {
                Seed = "Invalid seed format!",
                SequenceEndTime = 1,
                SequenceStartTime = 0
            });

            // Act
            ActionResult controllerResponse = await this._controller
                .PutAsync(requestObj, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="SeedReportController.PutAsync(SelfReportRequest, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> with invalid <see cref="Region"/> provided  
        /// with request
        /// </summary>
        [TestMethod]
        public async Task PutAsync_BadRequestObjectWithNoRegion()
        {
            // Arrange
            SelfReportRequest requestObj = new SelfReportRequest();
            requestObj.Seeds.Add(new BlueToothSeed
            {
                Seed = "00000000-0000-0000-0000-000000000000",
                SequenceEndTime = 1,
                SequenceStartTime = 0
            });

            // Act
            ActionResult controllerResponse = await this._controller
                .PutAsync(requestObj, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="SeedReportController.PutAsync(SelfReportRequest, CancellationToken)"/> 
        /// returns <see cref="BadRequestObjectResult"/> when no <see cref="Seed"/> objects are provided 
        /// with request
        /// </summary>
        [TestMethod]
        public async Task PutAsync_BadRequestObjectWithNoSeeds()
        {
            // Arrange
            SelfReportRequest requestObj = new SelfReportRequest
            {
                Region = new Region
                {
                    LatitudePrefix = 10.1234,
                    LongitudePrefix = 10.1234,
                    Precision = 4
                }
            };

            // Act
            ActionResult controllerResponse = await this._controller
                .PutAsync(requestObj, CancellationToken.None);

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestObjectResult));
        }

        /// <summary>
        /// <see cref="SeedReportController.PutAsync(SelfReportRequest, CancellationToken)"/> 
        /// returns <see cref="OkResult"/> with valid input data
        /// </summary>
        [TestMethod]
        public async Task PutAsync_OkWithValidInputs()
        {
            // Arrange
            SelfReportRequest requestObj = new SelfReportRequest
            {
                Region = new Region
                {
                    LatitudePrefix = 10.1234,
                    LongitudePrefix = 10.1234,
                    Precision = 4
                }
            };
            requestObj.Seeds.Add(new BlueToothSeed
            {
                SequenceEndTime = 1,
                SequenceStartTime = 0,
                Seed = "00000000-0000-0000-0000-000000000000"
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
