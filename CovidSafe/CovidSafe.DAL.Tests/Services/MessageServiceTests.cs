using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CovidSafe.DAL.Repositories;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Protos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CovidSafe.DAL.Tests.Services
{
    /// <summary>
    /// Unit Tests for the <see cref="MessageService"/> class
    /// </summary>
    [TestClass]
    public class MessageServiceTests
    {
        /// <summary>
        /// Mock <see cref="IMatchMessageRepository"/> instance
        /// </summary>
        private Mock<IMatchMessageRepository> _repo;
        /// <summary>
        /// <see cref="IMessageService"/> implementation being tested
        /// </summary>
        private MessageService _service;

        /// <summary>
        /// Initializes a new <see cref="MessageServiceTests"/> instance
        /// </summary>
        public MessageServiceTests()
        {
            // Initialize repositories
            this._repo = new Mock<IMatchMessageRepository>();

            // Create service
            this._service = new MessageService(this._repo.Object);
        }

        /// <summary>
        /// <see cref="MessageService.GetByIdsAsync(IEnumerable{string}, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with empty 'ids' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetByIdsAsync_ArgumentNullExceptionOnEmptyParameters()
        {
            // Arrange
            IEnumerable<string> ids = new List<string>();

            // Act
            IEnumerable<MatchMessage> result = await this._service
                .GetByIdsAsync(ids, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }

        /// <summary>
        /// <see cref="MessageService.GetByIdsAsync(IEnumerable{string}, CancellationToken)"/> 
        /// throws <see cref="ArgumentNullException"/> with null 'ids' parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetByIdsAsync_ArgumentNullExceptionOnNullParameters()
        {
            // Arrange
            // N/A

            // Act
            IEnumerable<MatchMessage> result = await this._service
                .GetByIdsAsync(null, CancellationToken.None);

            // Assert
            // Exception caught by decorator
        }
    }
}
