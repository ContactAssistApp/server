using System;
using System.Linq;
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
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/Messages/[controller]")]
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
        ///             "sequenceStartTime": 1586406048649,
        ///             "sequenceEndTime": 1586408048649
        ///         }],
        ///         "region": {
        ///             "latitudePrefix": 74.12,
        ///             "longitudePrefix": -39.12,
        ///             "precision": 2
        ///         },
        ///         "clientTimestamp": 1586409048649
        ///     }
        ///
        /// </remarks>
        /// <param name="request"><see cref="SelfReportRequest"/> content</param>
        /// <param name="cancellationToken">Cancellation token (not required in API call)</param>
        /// <response code="200">Query matched Trace results</response>
        /// <response code="400">Malformed or invalid query provided</response>
        [HttpPut]
        [Consumes("application/x-protobuf", "application/json")]
        [Produces("application/x-protobuf", "application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> PutAsync(SelfReportRequest request, CancellationToken cancellationToken = default)
        {
            // Get server timestamp at request immediately
            long serverTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // Validate inputs
            if (request == null || request.ClientTimestamp < 0 || request.Region == null || request.Seeds.Count() == 0)
            {
                return BadRequest();
            }

            // Validate seed formats (can they parse to Guid?)
            foreach(BlueToothSeed seed in request.Seeds)
            {
                Guid output;
                if(!Guid.TryParse(seed.Seed, out output))
                {
                    return BadRequest(String.Format("'{0}' is not a valid GUID/UUID.", seed.Seed));
                }
            }

            //TODO: add proper region validation
            //TODO: remove precision limitation
            if (request.Region.Precision != 4)
            {
                return BadRequest("Only precision 4 is supported for insertion temporarily");
            }

            await this._messageService.PublishAsync(request, serverTimestamp, cancellationToken);

            return Ok();
        }
    }
}