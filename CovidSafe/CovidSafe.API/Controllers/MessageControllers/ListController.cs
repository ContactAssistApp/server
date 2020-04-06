using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.DAL.Services;
using CovidSafe.Entities.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidSafe.API.Controllers.MessageControllers
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
        private IMessageService _messageService;

        /// <summary>
        /// Creates a new <see cref="ListController"/> instance
        /// </summary>
        /// <param name="messageService"><see cref="MatchMessage"/> service layer</param>
        public ListController(IMessageService messageService)
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
        ///     GET /Messages/List?lat=74.12&amp;lon=-39.12&amp;precision=2&amp;lastTimestamp=0
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
        [Produces("application/x-protobuf", "application/json")]
        [ProducesResponseType(typeof(MessageListResponse), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<MessageListResponse>> GetAsync([Required] double lat, [Required] double lon, [Required] int precision, [Required] long lastTimestamp)
        {
            CancellationToken ct = new CancellationToken();

            // Validate inputs
            // Latitudes are from -90 to 90
            if(lat > 90 || lat < -90)
            {
                return BadRequest();
            }
            // Longitudes are from -180 to 180
            if(lon > 180 || lon < -180)
            {
                return BadRequest();
            }
            // Precision can be max 8
            if(precision < 0 || precision > 8)
            {
                return BadRequest();
            }
            if(lastTimestamp < 0)
            {
                return BadRequest();
            }

            // Pull queries matching parameters
            var region = new Region { LatitudePrefix = lat, LongitudePrefix = lon, Precision = precision };
            IEnumerable<MessageInfo> results = await this._messageService
                .GetLatestInfoAsync(region, lastTimestamp, ct);

            if (results != null)
            {
                // Convert to response proto
                MessageListResponse response = new MessageListResponse();
                response.MessageInfoes.AddRange(results);

                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }
    }
}