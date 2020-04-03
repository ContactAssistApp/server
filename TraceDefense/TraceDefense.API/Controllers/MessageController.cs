using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.DAL.Services;
using TraceDefense.Entities.Protos;

namespace TraceDefense.API.Controllers
{
    /// <summary>
    /// Handles <see cref="MatchMessage"/> CRUD operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        /// <summary>
        /// <see cref="MatchMessage"/> service layer
        /// </summary>
        private IMessageService _messageService;

        /// <summary>
        /// Creates a new <see cref="MessageController"/> instance
        /// </summary>
        /// <param name="messageService"><see cref="MatchMessage"/> service layer</param>
        public MessageController(IMessageService messageService)
        {
            // Assign local values
            this._messageService = messageService;
        }

        /// <summary>
        /// Get <see cref="MatchMessage"/> objects matching the provided identifiers
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Query
        ///     {
        ///         "RequestedQueries": [{
        ///             "QueryId": "00000000-0000-0000-0000-000000000000",
        ///             "QueryTimestamp": {
        ///                 "year": 2020,
        ///                 "month": 3,
        ///                 "day": 31,
        ///                 "hour": 12,
        ///                 "minute": 23,
        ///                 "second": 10,
        ///                 "millisecond": 12
        ///             }
        ///         }]
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <response code="404">No results found for request parameters</response>
        /// <param name="request"><see cref="MessageRequest"/> parameters</param>
        /// <returns>Collection of <see cref="MatchMessage"/> objects matching request parameters</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<MessageResponse>> PostAsync([FromBody] MessageRequest request)
        {
            CancellationToken ct = new CancellationToken();

            // Validate inputs
            if(request == null)
            {
                return BadRequest();
            }

            // Get results
            IEnumerable<string> requestedIds = request.RequestedQueries
                   .Select(r => r.MessageId);
            IEnumerable<MatchMessage> result = await this._messageService
                .GetByIdsAsync(requestedIds, ct);

            if(result.Count() > 0)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Publish a <see cref="MatchMessage"/> for distribution among devices relevant to 
        /// <see cref="Region"/>
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Message
        ///     {
        ///         "messageVersion": "1.0.0",
        ///         "geoProximity": [
        ///             {
        ///                 "userMessage": "Quarantine at home if you were at Trader Joe's on 128th street.",
        ///                 "locations": [
        ///                     {
        ///                         "location": {
        ///                             "lattitude": -39.1234,
        ///                             "longitude": 47.1231,
        ///                             "radiusMeters": 100
        ///                         },
        ///                         "time": {
        ///                             "year": 2020,
        ///                             "month": 3,
        ///                             "day": 31,
        ///                             "hour": 12,
        ///                             "minute": 30,
        ///                             "second": 12,
        ///                             "millisecond": 32
        ///                         }
        ///                     }
        ///                 ],
        ///                 "proximityRadiusMeters": 1000,
        ///                 "durationToleranceSecs": 600
        ///             }
        ///         ],
        ///         "idList": [
        ///             {
        ///                 "userMessage": "These IDs might give you a bad time!",
        ///                 "ids": [
        ///                     "ABCDEF1234567890"
        ///                 ]
        ///             }
        ///         ]
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Query matched Trace results</response>
        /// <response code="400">Malformed or invalid query provided</response>
        /// <response code="404">No query results</response>
        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> PutAsync(AnnounceRequest request)
        {
            CancellationToken ct = new CancellationToken();
            await this._messageService.PublishAsync(request.Region, request.Message, ct);
            return Ok();
        }
    }
}