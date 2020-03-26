using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.Entities;

namespace TraceDefense.API.Controllers
{
    /// <summary>
    /// Allows client devices to check status on proximity to known infections
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        /// <summary>
        /// Checks status of device proximity to known infections
        /// </summary>
        /// <param name="deviceId">Unique device identifier</param>
        /// <response code="200">Status retrieved successfully</response>
        /// <response code="404">Incorrect device identifier provided</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> GetAsync(string deviceId)
        {
            CancellationToken ct = new CancellationToken();

            // Validate inputs
            if(String.IsNullOrEmpty(deviceId))
            {
                return BadRequest();
            }

            // Get device status
            // TODO: Get status from data repository
            return Ok(ProximityStatus.NONE);
        }
    }
}