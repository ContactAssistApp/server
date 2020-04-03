using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.DAL.Services;
using TraceDefense.Entities.Protos;

namespace TraceDefense.API.Controllers.MessageControllers
{
    /// <summary>
    /// Handles requests to list <see cref="MatchMessage"/> identifiers which are new to a client
    /// </summary>
    [Route("api/Messages/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        /// <summary>
        /// <see cref="MatchMessage"/> service layer
        /// </summary>
        private IProximityQueryService _messageService;

        /// <summary>
        /// Creates a new <see cref="ListController"/> instance
        /// </summary>
        /// <param name="messageService"><see cref="MatchMessage"/> service layer</param>
        public ListController(IProximityQueryService messageService)
        {
            // Assign local values
            this._messageService = messageService;
        }

        /// <summary>
        /// Get <see cref="MessageInfo"/> for a region, starting at a provided timestamp
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Messages/List?regionId=39%2c-74&amp;lastTimestamp=0
        ///     
        /// </remarks>
        /// <param name="lat">Latitude of desired <see cref="Region"/></param>
        /// <param name="lon">Longitude of desired <see cref="Region"/></param>
        /// <param name="precision">Precision of desired <see cref="Region"/></param>
        /// <param name="lastTimestamp">Latest <see cref="MatchMessage"/> timestamp on client device, in ms from UNIX epoch</param>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <response code="404">No results found for request parameters</response>
        /// <returns>Collection of <see cref="MessageInfo"/> objects matching request parameters</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageListResponse), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<MessageListResponse>> GetAsync(double lat, double lon, int precision, long lastTimestamp)
        {
            CancellationToken ct = new CancellationToken();

            // Validate inputs
            var region = new Region { LattitudePrefix = lat, LongitudePrefix = lon, Precision = precision };
            if(lastTimestamp < 0)
            {
                return BadRequest();
            }

            // Pull queries matching parameters
            IEnumerable<MessageInfo> results = await this._messageService
                .GetLatestInfoAsync(region, lastTimestamp, ct);

            if (results != null)
            {
                // Convert to response proto
                MessageListResponse response = new MessageListResponse();
                response.MessageInfo.AddRange(results);

                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }
    }
}