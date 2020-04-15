using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.DAL.Services;
using CovidSafe.Entities.Protos;
using CovidSafe.Entities.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidSafe.API.v1.Controllers.MessageControllers
{
    /// <summary>
    /// Handles requests to list <see cref="MatchMessage"/> identifiers which are new to a client
    /// </summary>
    [ApiController]
    [ApiVersion("1", Deprecated = true)]
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/Messages/[controller]")]
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
        /// <param name="cancellationToken">Cancellation token (not required in API call)</param>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <returns>Collection of <see cref="MessageInfo"/> objects matching request parameters</returns>
        [HttpGet]
        [Produces("application/x-protobuf", "application/json")]
        [ProducesResponseType(typeof(MessageListResponse), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<MessageListResponse>> GetAsync([Required] double lat, [Required] double lon, [Required] int precision, [Required] long lastTimestamp, CancellationToken cancellationToken = default)
        {
            try
            {
                // Pull queries matching parameters
                var region = new Region { LatitudePrefix = lat, LongitudePrefix = lon, Precision = precision };
                IEnumerable<MessageInfo> results = await this._messageService
                    .GetLatestInfoAsync(region, lastTimestamp, cancellationToken);

                // Convert to response proto
                MessageListResponse response = new MessageListResponse();

                if(results.Count() > 0)
                {
                    response.MessageInfoes.AddRange(results);

                    // Get maximum timestamp from resultset
                    response.MaxResponseTimestamp = response.MessageInfoes.Max(m => m.MessageTimestamp);
                }

                return Ok(response);
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

        /// <summary>
        /// Get total size of <see cref="MatchMessage"/> objects for a <see cref="Region"/> based 
        /// on the provided parameters when using application/x-protobuf
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     HEAD /Messages/List?lat=74.12&amp;lon=-39.12&amp;precision=2&amp;lastTimestamp=0
        ///     
        /// </remarks>
        /// <param name="lat">Latitude of desired <see cref="Region"/></param>
        /// <param name="lon">Longitude of desired <see cref="Region"/></param>
        /// <param name="precision">Precision of desired <see cref="Region"/></param>
        /// <param name="lastTimestamp">Latest <see cref="MatchMessage"/> timestamp on client device, in ms from UNIX epoch</param>
        /// <param name="cancellationToken">Cancellation token (not required in API call)</param>
        /// <response code="200">Successful request</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <returns>
        /// Total size of matching <see cref="MatchMessage"/> objects (via Content-Type header), in bytes, based 
        /// on their size when converted to the Protobuf format
        /// </returns>
        [ApiVersion("1.1")]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> HeadAsync([Required] double lat, [Required] double lon, [Required] int precision, [Required] long lastTimestamp, CancellationToken cancellationToken = default)
        {
            try
            {
                // Pull queries matching parameters
                var region = new Region { LatitudePrefix = lat, LongitudePrefix = lon, Precision = precision };
                long size = await this._messageService
                    .GetLatestRegionDataSizeAsync(region, lastTimestamp, cancellationToken);

                // Set Content-Length header with calculated size
                Response.ContentLength = size;

                return Ok();
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