using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using CovidSafe.API.v20200415.Protos;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Reports;
using CovidSafe.Entities.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidSafe.API.v20200415.Controllers
{
    /// <summary>
    /// Handles <see cref="MatchMessage"/> CRUD operations
    /// </summary>
    [ApiController]
    [ApiVersion("2020-04-15", Deprecated = true)]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        /// <summary>
        /// AutoMapper instance for object resolution
        /// </summary>
        private readonly IMapper _map;
        /// <summary>
        /// <see cref="InfectionReport"/> service layer
        /// </summary>
        private readonly IInfectionReportService _reportService;

        /// <summary>
        /// Creates a new <see cref="MessageController"/> instance
        /// </summary>
        /// <param name="map">AutoMapper instance</param>
        /// <param name="reportService"><see cref="InfectionReport"/> service layer</param>
        public MessageController(IMapper map, IInfectionReportService reportService)
        {
            // Assign local values
            this._map = map;
            this._reportService = reportService;
        }

        /// <summary>
        /// Get <see cref="MatchMessage"/> objects matching the provided identifiers
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Message&amp;api-version=2020-04-15
        ///     {
        ///         "RequestedQueries": [{
        ///             "messageId": "baa0ebe1-e6dd-447d-8d82-507644991e07",
        ///             "messageTimestamp": 1586199635012
        ///         }]
        ///     }
        ///     
        /// </remarks>
        /// <param name="request"><see cref="MessageRequest"/> parameters</param>
        /// <param name="cancellationToken">Cancellation token (not required in API call)</param>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <returns>Collection of <see cref="MatchMessage"/> objects matching request parameters</returns>
        [HttpPost]
        [Consumes("application/x-protobuf", "application/json")]
        [Produces("application/x-protobuf", "application/json")]
        [ProducesResponseType(typeof(IEnumerable<MatchMessage>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RequestValidationResult), StatusCodes.Status400BadRequest)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<IEnumerable<MatchMessage>>> PostAsync([FromBody] MessageRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                // Submit request
                IEnumerable<InfectionReport> reports = await this._reportService
                    .GetByIdsAsync(
                        request.RequestedQueries.Select(r => r.MessageId),
                        cancellationToken
                );

                // Map MatchMessage types
                List<MatchMessage> messages = new List<MatchMessage>();

                foreach(InfectionReport report in reports)
                {
                    MatchMessage result = this._map.Map<MatchMessage>(report);
                    // Get BLEs
                    BluetoothMatch match = new BluetoothMatch();
                    match.Seeds.AddRange(
                        report.BluetoothSeeds.Select(s => this._map.Map<BlueToothSeed>(s))
                    );
                    // Add converted BLE match
                    result.BluetoothMatches.Add(match);
                    // Add converted MatchMessage
                    messages.Add(result);
                }

                // Return as expected proto type
                return Ok(messages);
            }
            catch(RequestValidationFailedException ex)
            {
                // Only return validation results
                return BadRequest(ex.ValidationResult);
            }
            catch(ArgumentNullException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Service status request endpoint, used mostly by Azure services to determine if 
        /// an endpoint is alive
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     HEAD /api/Message
        /// 
        /// </remarks>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <response code="200">Successful request</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public Task<OkResult> HeadAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Ok());
        }
    }
}