using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.Entities;

namespace TraceDefense.API.Controllers.Trace
{
    /// <summary>
    /// Uploads a new <see cref="TraceEvent"/> entry for a device
    /// </summary>
    [Route("api/Trace/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        /// <summary>
        /// Submits a query for <see cref="TraceEvent"/> objects
        /// </summary>
        /// <response code="200"><see cref="TraceEvent"/> successfully submitted</response>
        /// <response code="400">Malformed or invalid <see cref="TraceEvent"/> provided</response>
        /// <response code="404">Invalid or unknown device identifier presented</response>
        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> PutAsync()
        {
            CancellationToken ct = new CancellationToken();

            // TODO: Validate inputs

            // TODO: Upload trace

            return Ok();
        }
    }
}