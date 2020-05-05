using System;
using System.Threading;
using System.Threading.Tasks;

using CovidSafe.DAL.Services;
using CovidSafe.Entities.Protos.v20200415;
using CovidSafe.Entities.Reports;
using CovidSafe.Entities.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidSafe.API.v20200415.Controllers.MessageControllers
{
    /// <summary>
    /// Handles requests for infected clients volunteering <see cref="BlueToothSeed"/> identifiers
    /// </summary>
    [ApiController]
    [ApiVersion("2020-04-15", Deprecated = true)]
    [Route("api/Messages/[controller]")]
    public class SeedReportController : ControllerBase
    {
        /// <summary>
        /// <see cref="InfectionReport"/> service layer
        /// </summary>
        private IInfectionReportService _reportService;

        /// <summary>
        /// Creates a new <see cref="SeedReportController"/> instance
        /// </summary>
        /// <param name="reportService"><see cref="InfectionReport"/> service layer</param>
        public SeedReportController(IInfectionReportService reportService)
        {
            // Assign local values
            this._reportService = reportService;
        }

        /// <summary>
        /// Publish a <see cref="SelfReportRequest"/> for distribution among devices relevant to 
        /// <see cref="Region"/>
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/Messages/SeedReport&amp;api-version={current_version}
        ///     {
        ///         "seeds": [{
        ///             "seed": "00000000-0000-0000-0000-000000000000",
        ///             "sequenceStartTime": 1586406048649,
        ///             "sequenceEndTime": 1586408048649
        ///         }],
        ///         "region": {
        ///             "latitudePrefix": 74.12,
        ///             "longitudePrefix": -39.12,
        ///             "precision": 2
        ///         }
        ///     }
        ///
        /// </remarks>
        /// <param name="request"><see cref="SelfReportRequest"/> content</param>
        /// <param name="cancellationToken">Cancellation token (not required in API call)</param>
        /// <response code="200">Submission successful</response>
        /// <response code="400">Malformed or invalid request</response>
        [HttpPut]
        [Consumes("application/x-protobuf", "application/json")]
        [Produces("application/x-protobuf", "application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RequestValidationResult), StatusCodes.Status400BadRequest)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> PutAsync(SelfReportRequest request, CancellationToken cancellationToken = default)
        {
            // Get server timestamp at request immediately
            long serverTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            try
            {
                await this._reportService.PublishAsync(request, serverTimestamp, cancellationToken);
                return Ok();
            }
            catch (RequestValidationFailedException ex)
            {
                // Only return validation results
                return BadRequest(ex.ValidationResult);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }
    }
}