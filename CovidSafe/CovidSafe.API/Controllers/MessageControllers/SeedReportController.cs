using System.Threading;
using System.Threading.Tasks;

using CovidSafe.DAL.Services;
using CovidSafe.Entities.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidSafe.API.Controllers.MessageControllers
{
    /// <summary>
    /// Handles requests for infected clients volunteering <see cref="BlueToothSeed"/> identifiers
    /// </summary>
    [Route("api/Messages/[controller]")]
    [ApiController]
    public class SeedReportController : ControllerBase
    {
        /// <summary>
        /// <see cref="MatchMessage"/> service layer
        /// </summary>
        private IMessageService _messageService;

        /// <summary>
        /// Creates a new <see cref="SeedReportController"/> instance
        /// </summary>
        /// <param name="messageService"><see cref="MatchMessage"/> service layer</param>
        public SeedReportController(IMessageService messageService)
        {
            // Assign local values
            this._messageService = messageService;
        }

        /// <summary>
        /// Publish a <see cref="SelfReportRequest"/> for distribution among devices relevant to 
        /// <see cref="Region"/>
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Messages/SeedReport
        ///     {
        ///         "seeds": [{
        ///             "seed": "00000000-0000-0000-0000-000000000000",
        ///             "sequenceStartTime": 1586044800,
        ///             "sequenceEndTime": 1586048400
        ///         }],
        ///         "region": {
        ///             "lattitudePrefix": 74.12,
        ///             "longitudePrefix": -39.12,
        ///             "precision": 2
        ///         }
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Query matched Trace results</response>
        /// <response code="400">Malformed or invalid query provided</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> PutAsync(SelfReportRequest request)
        {
            CancellationToken ct = new CancellationToken();

            // Validate inputs
            if (request == null)
            {
                return BadRequest();
            }
            //TODO: add proper region validation
            //TODO: remove precision limitation
            if (request.Region.Precision != 4)
            {
                return BadRequest("Only precision 4 is supported for insertion temporarily");
            }

            await this._messageService.PublishAsync(request.Region, request.Seeds, ct);

            return Ok();
        }
    }
}