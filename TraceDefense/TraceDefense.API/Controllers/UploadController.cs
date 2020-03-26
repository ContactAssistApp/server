using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.Entities;

namespace TraceDefense.API.Controllers
{
    /// <summary>
    /// Endpoint for receiving user device <see cref="TraceEvent"/> entities
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        /// <summary>
        /// Pushes <see cref="TraceEvent"/> entities to the server
        /// </summary>
        /// <param name="deviceId">Unique device identifier</param>
        /// <param name="traces">Collection of <see cref="TraceEvent"/> entities</param>
        /// <response code="200"><see cref="TraceEvent"/> entities uploaded successfully</response>
        /// <resposne code="400">Missing or invalid request data provided by client</resposne>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> PostAsync(string deviceId, IEnumerable<TraceEvent> traces)
        {
            CancellationToken ct = new CancellationToken();

            // Validate inputs
            if(String.IsNullOrEmpty(deviceId))
            {
                return BadRequest();
            }

            if(traces.Count() == 0)
            {
                return BadRequest();
            }

            // Upload TraceEvent objects
            // TODO: Upload objects to data repository
            return Ok();
        }
    }
}