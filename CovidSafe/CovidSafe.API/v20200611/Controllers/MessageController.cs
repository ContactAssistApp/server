using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using CovidSafe.API.v20200611.Protos;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Messages;
using CovidSafe.Entities.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidSafe.API.v20200611.Controllers
{
    /// <summary>
    /// Handles <see cref="MessageContainer"/> CRUD operations
    /// </summary>
    [ApiController]
    [ApiVersion("2020-06-11")]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        /// <summary>
        /// AutoMapper instance for object resolution
        /// </summary>
        private readonly IMapper _map;
        /// <summary>
        /// <see cref="MessageContainer"/> service layer
        /// </summary>
        private readonly IMessageService _reportService;

        /// <summary>
        /// Creates a new <see cref="MessageController"/> instance
        /// </summary>
        /// <param name="map">AutoMapper instance</param>
        /// <param name="reportService"><see cref="MessageContainer"/> service layer</param>
        public MessageController(IMapper map, IMessageService reportService)
        {
            // Assign local values
            this._map = map;
            this._reportService = reportService;
        }

        /// <summary>
        /// Get <see cref="MessageContainer"/> objects matching the provided identifiers
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Message&amp;api-version=2020-06-11
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
        /// <returns><see cref="MessageResponse"/> of reports matching provided request parameters</returns>
        [HttpPost]
        [Consumes("application/x-protobuf", "application/json")]
        [Produces("application/x-protobuf", "application/json")]
        [ProducesResponseType(typeof(IEnumerable<MessageResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Protos.RequestValidationResult), StatusCodes.Status400BadRequest)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<MessageResponse>> PostAsync([FromBody] MessageRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                // Submit request
                IEnumerable<MessageContainer> reports = await this._reportService
                    .GetByIdsAsync(
                        request.RequestedQueries.Select(r => r.MessageId),
                        cancellationToken
                );

                // Map results to expected return type
                MessageResponse response = new MessageResponse();
                foreach(MessageContainer container in reports)
                {
                    response.NarrowcastMessages.AddRange(
                        container.Narrowcasts.Select(
                            c => this._map.Map<Protos.NarrowcastMessage>(c)
                        )
                    );
                }

                return Ok(response);
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