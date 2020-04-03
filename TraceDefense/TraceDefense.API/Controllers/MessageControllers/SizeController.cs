using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.DAL.Services;
using TraceDefense.Entities.Protos;

namespace TraceDefense.API.Controllers.MessageControllers
{
    /// <summary>
    /// Handles <see cref="MatchMessage"/> size requests
    /// </summary>
    [Route("api/Messages/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        /// <summary>
        /// <see cref="MatchMessage"/> service layer
        /// </summary>
        private IProximityQueryService _messageService;

        /// <summary>
        /// Creates a new <see cref="SizeController"/> instance
        /// </summary>
        /// <param name="messageService"><see cref="MatchMessage"/> service layer</param>
        public SizeController(IProximityQueryService messageService)
        {
            // Assign local values
            this._messageService = messageService;
        }

        /// <summary>
        /// Gets the total data size for a provided <see cref="Region"/> based on client 
        /// parameters
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Query/Size?regionId=39%2c-74&amp;lastTimestamp=0
        ///     
        /// </remarks>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <response code="404">No results found for request parameters</response>
        /// <param name="lat">Latitude of desired <see cref="Region"/></param>
        /// <param name="lon">Longitude of desired <see cref="Region"/></param>
        /// <param name="precision">Precision of desired <see cref="Region"/></param>
        /// <param name="lastTimestamp">Timestamp of client's most recent <see cref="MatchMessage"/>, in ms since UNIX epoch</param>
        /// <returns>Total size of target <see cref="MatchMessage"/> objects, in bytes</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageSizeResponse), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<MessageSizeResponse>> GetAsync(double lat, double lon, int precision, long lastTimestamp)
        {
            CancellationToken ct = new CancellationToken();

            // Validate inputs
            var region = new Region { LattitudePrefix = lat, LongitudePrefix = lon, Precision = precision };

            if(lastTimestamp < 0)
            {
                return BadRequest();
            }

            // Get results
            long result = await this._messageService.GetLatestRegionDataSizeAsync(region, lastTimestamp, ct);

            if(result > 0)
            {
                return Ok(new MessageSizeResponse
                {
                    SizeOfQueryResponse = result
                });
            }
            else
            {
                return NotFound();
            }
        }
    }
}