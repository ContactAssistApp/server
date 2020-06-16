using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using CovidSafe.API.v20200505.Protos;
using CovidSafe.DAL.Helpers;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Messages;
using CovidSafe.Entities.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidSafe.API.v20200505.Controllers.MessageControllers
{
    /// <summary>
    /// Handles requests to list <see cref="MatchMessage"/> identifiers which are new to a client
    /// </summary>
    [ApiController]
    [ApiVersion("2020-05-05", Deprecated = true)]
    [Route("api/Messages/[controller]")]
    public class ListController : ControllerBase
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
        /// Creates a new <see cref="ListController"/> instance
        /// </summary>
        /// <param name="map">AutoMapper instance</param>
        /// <param name="reportService"><see cref="MessageContainer"/> service layer</param>
        public ListController(IMapper map, IMessageService reportService)
        {
            // Assign local values
            this._map = map;
            this._reportService = reportService;
        }

        /// <summary>
        /// Get <see cref="MessageInfo"/> for a region, starting at a provided timestamp
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/Messages/List?lat=74.12&amp;lon=-39.12&amp;precision=2&amp;lastTimestamp=0&amp;api-version=2020-05-05
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
        [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<MessageListResponse>> GetAsync([Required] double lat, [Required] double lon, [Required] int precision, [Required] long lastTimestamp, CancellationToken cancellationToken = default)
        {
            try
            {
                // Pull queries matching parameters. Legacy precision is ignored
                var region =RegionHelper.CreateRegion(lat, lon);

                IEnumerable<MessageContainerMetadata> results = await this._reportService
                    .GetLatestInfoAsync(region, lastTimestamp, cancellationToken);

                // Return using mapped proto object
                return Ok(this._map.Map<MessageListResponse>(results));
            }
            catch (RequestValidationFailedException ex)
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
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> HeadAsync([Required] double lat, [Required] double lon, [Required] int precision, [Required] long lastTimestamp, CancellationToken cancellationToken = default)
        {
            try
            {
                // Pull queries matching parameters
                var region = RegionHelper.CreateRegion(lat, lon);

                long size = await this._reportService
                    .GetLatestRegionDataSizeAsync(region, lastTimestamp, cancellationToken);

                // Set Content-Length header with calculated size
                Response.ContentLength = size;

                return Ok();
            }
            catch (RequestValidationFailedException ex)
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