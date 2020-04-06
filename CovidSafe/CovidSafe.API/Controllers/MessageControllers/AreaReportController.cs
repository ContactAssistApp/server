using System.Threading.Tasks;

using CovidSafe.DAL.Services;
using CovidSafe.Entities.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidSafe.API.Controllers.MessageControllers
{
    /// <summary>
    /// Handles requests for infected clients volunteering <see cref="AreaMatch"/> identifiers
    /// </summary>
    [Route("api/Messages/[controller]")]
    [ApiController]
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
        /// Publish a <see cref="AreaMatch"/> for distribution among devices relevant to 
        /// <see cref="Region"/>
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Messages/AreaReport
        ///     {
        ///         "areaMatches": [{
        ///             "userMessage": "Monitor symptoms for one week.",
        ///             "areas": [{
        ///                 "location": {
        ///                     "lattitude": 74.12345,
        ///                     "longitude": -39.12345
        ///                 },
        ///                 "radiusMeters": 100,
        ///                 "beginTime": 1586083599,
        ///                 "endTime": 1586085189
        ///             }]
        ///         }],
        ///         "region": {
        ///             "latitudePrefix": 74.12,
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
        public async Task<ActionResult> PutAsync(AreaMatch request)
        {
            // Blocked for now
            return NotFound();
        }
    }
}