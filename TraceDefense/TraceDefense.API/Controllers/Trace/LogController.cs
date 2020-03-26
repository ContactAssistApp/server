using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TraceDefense.API.Controllers.Trace
{
    /// <summary>
    /// Uploads a new <see cref="Entities.Trace"/> entry for a device
    /// </summary>
    [Route("api/Trace/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        /// <summary>
        /// Submits a query for <see cref="Entities.Trace"/> objects
        /// </summary>
        /// <param name="traceEvent">Trace event parameters</param>
        /// <response code="200"><see cref="Entities.Trace"/> successfully submitted</response>
        /// <response code="400">Malformed or invalid <see cref="Entities.Trace"/> provided</response>
        /// <response code="404">Invalid or unknown device identifier presented</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> PutAsync([FromBody] Entities.Trace traceEvent)
        {
            CancellationToken ct = new CancellationToken();

            // TODO: Validate inputs
            if(traceEvent != null)
            {
                return BadRequest();
            }

            // TODO: Upload trace

            return Ok();
        }
    }
}