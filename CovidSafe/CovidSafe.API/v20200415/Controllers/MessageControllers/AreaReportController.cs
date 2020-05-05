using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.DAL.Services;
using CovidSafe.Entities.Protos;
using CovidSafe.Entities.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidSafe.API.v20200415.Controllers.MessageControllers
{
    /// <summary>
    /// Handles requests for infected clients volunteering <see cref="AreaMatch"/> identifiers
    /// </summary>
    [ApiController]
    [Obsolete]
    [Route("api/Messages/[controller]")]
    public class AreaReportController : ControllerBase
    {
        /// <summary>
        /// <see cref="MatchMessage"/> service layer
        /// </summary>
        private IMessageService _messageService;

        /// <summary>
        /// Creates a new <see cref="AreaReportController"/> instance
        /// </summary>
        /// <param name="messageService"><see cref="MatchMessage"/> service layer</param>
        public AreaReportController(IMessageService messageService)
        {
            // Assign local values
            this._messageService = messageService;
        }

        /// <summary>
        /// Publish a <see cref="AreaMatch"/> for distribution among devices
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/Messages/AreaReport&amp;api-version={current_version}
        ///     {
        ///         "userMessage": "Monitor symptoms for one week.",
        ///         "areas": [{
        ///             "location": {
        ///                 "latitude": 74.12345,
        ///                 "longitude": -39.12345
        ///             },
        ///             "radiusMeters": 100,
        ///             "beginTime": 1586083599,
        ///             "endTime": 1586085189
        ///         }]
        ///     }
        ///
        /// </remarks>
        /// <param name="request"><see cref="AreaMatch"/> to be stored</param>
        /// <param name="cancellationToken">Cancellation token (not required in API call)</param>
        /// <response code="200">Submission successful</response>
        /// <response code="400">Malformed or invalid request</response>
        [HttpPut]
        [Consumes("application/x-protobuf", "application/json")]
        [Produces("application/x-protobuf", "application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> PutAsync([Required] AreaMatch request, CancellationToken cancellationToken = default)
        {
            try
            {
                // Publish area
                await this._messageService.PublishAreaAsync(request, cancellationToken);
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