using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using CovidSafe.API.v20200611.Protos;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Messages;
using CovidSafe.Entities.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidSafe.API.v20200611.Controllers.MessageControllers
{
    /// <summary>
    /// Handles <see cref="NarrowcastMessage"/> announcements
    /// </summary>
    [ApiController]
    [ApiVersion("2020-06-11")]
    [Authorize]
    [Route("api/Messages/[controller]")]
    public class AnnounceController : ControllerBase
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
        /// Creates a new <see cref="AnnounceController"/> instance
        /// </summary>
        /// <param name="map">AutoMapper instance</param>
        /// <param name="reportService"><see cref="MessageContainer"/> service layer</param>
        public AnnounceController(IMapper map, IMessageService reportService)
        {
            // Assign local values
            this._map = map;
            this._reportService = reportService;
        }

        /// <summary>
        /// Publish a <see cref="NarrowcastMessage"/> for distribution among devices
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/Messages/Announce&amp;api-version=2020-06-11
        ///     {
        ///         "userMessage": "Monitor symptoms for one week.",
        ///         "area": {
        ///             "location": {
        ///                 "latitude": 74.12345,
        ///                 "longitude": -39.12345
        ///             },
        ///             "radiusMeters": 100,
        ///             "beginTime": 1591997285105,
        ///             "endTime": 1591997385105
        ///         }
        ///     }
        ///
        /// </remarks>
        /// <param name="request"><see cref="NarrowcastMessage"/> to be stored</param>
        /// <param name="cancellationToken">Cancellation token (not required in API call)</param>
        /// <response code="200">Submission successful</response>
        /// <response code="400">Malformed or invalid request</response>
        [HttpPut]
        [Consumes("application/x-protobuf", "application/json")]
        [Produces("application/x-protobuf", "application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> PutAsync([Required] Protos.NarrowcastMessage request, CancellationToken cancellationToken = default)
        {
            try
            {
                // Parse AreaMatch to AreaReport type
                Entities.Messages.NarrowcastMessage report = this._map.Map<Entities.Messages.NarrowcastMessage>(request);

                // Publish area
                await this._reportService.PublishAreaAsync(report, cancellationToken);
                return Ok();
            }
            catch (RequestValidationFailedException ex)
            {
                // Only return validation issues
                return BadRequest(ex.ValidationResult);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }
    }
}