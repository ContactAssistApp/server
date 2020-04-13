using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
    }
}
