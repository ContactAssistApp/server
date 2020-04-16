using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.DAL.Services;
using CovidSafe.Entities.Protos;
using CovidSafe.Entities.Protos.Deprecated;
using CovidSafe.Entities.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidSafe.API.Controllers.MessageControllers
{
    /// <summary>
    /// Handles <see cref="MatchMessage"/> size requests
    /// </summary>
    [ApiController]
    [ApiVersion("2020-04-14", Deprecated = true)]
    [Route("api/Messages/[controller]")]
    public class SizeController : ControllerBase
    {
        /// <summary>
        /// <see cref="MatchMessage"/> service layer
        /// </summary>
        private IMessageService _messageService;
        /// <summary>
        /// Default response provided when parameters have no matching output
        /// </summary>
        public const long NOT_FOUND_RESPONSE = -1;

        /// <summary>
        /// Creates a new <see cref="SizeController"/> instance
        /// </summary>
        /// <param name="messageService"><see cref="MatchMessage"/> service layer</param>
        public SizeController(IMessageService messageService)
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
        ///     GET /Messages/Size?lat=74.12&amp;lon=-39.12&amp;precision=2&amp;lastTimestamp=0
        ///     
        /// </remarks>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <param name="lat">Latitude of desired <see cref="Region"/></param>
        /// <param name="lon">Longitude of desired <see cref="Region"/></param>
        /// <param name="precision">Precision of desired <see cref="Region"/></param>
        /// <param name="lastTimestamp">Timestamp of client's most recent <see cref="MatchMessage"/>, in ms since UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token (not required in API call)</param>
        /// <returns>
        /// Total size of target <see cref="MatchMessage"/> objects, in bytes, or -1 if parameters are unmatched
        /// </returns>
        [HttpGet]
        [Produces("application/x-protobuf", "application/json")]
        [ProducesResponseType(typeof(MessageSizeResponse), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<MessageSizeResponse>> GetAsync([Required] double lat, [Required] double lon, [Required] int precision, [Required] long lastTimestamp, CancellationToken cancellationToken = default)
        {
            try
            {
                // Get results
                var region = new Region { LatitudePrefix = lat, LongitudePrefix = lon, Precision = precision };
                long result = await this._messageService.GetLatestRegionDataSizeAsync(region, lastTimestamp, cancellationToken);

                // Return -1 if no results
                if (result <= 0)
                {
                    result = NOT_FOUND_RESPONSE;
                }

                return Ok(new MessageSizeResponse
                {
                    SizeOfQueryResponse = result
                });
            }
            catch (ValidationFailedException ex)
            {
                return BadRequest(ex.ValidationResult);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }
    }
}