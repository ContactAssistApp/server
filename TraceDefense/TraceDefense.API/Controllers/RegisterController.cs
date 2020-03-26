using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.DAL.Repositories;
using TraceDefense.Entities.Registration;

namespace TraceDefense.API.Controllers
{
    /// <summary>
    /// API Controller for registering new devices with the backend service
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        /// <summary>
        /// Device registration repository instance
        /// </summary>
        private IDeviceRegistrationRepository _registrationRepo;

        /// <summary>
        /// Creates a new <see cref="RegisterController"/> instance
        /// </summary>
        /// <param name="registrationRepo"><see cref="DeviceRegistration"/> repository instance</param>
        public RegisterController(IDeviceRegistrationRepository registrationRepo)
        {
            this._registrationRepo = registrationRepo;
        }

        /// <summary>
        /// Registers a new device
        /// </summary>
        /// <param name="deviceId">Unique device identifier</param>
        /// <param name="region">Device granular region name</param>
        /// <response code="200">Registration succeeded</response>
        /// <response code="400">Missing or invalid request data provided by client</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> PutAsync(string deviceId, string region)
        {
            CancellationToken ct = new CancellationToken();

            // Validate inputs
            if(String.IsNullOrEmpty(deviceId))
            {
                return BadRequest();
            }

            if(String.IsNullOrEmpty(region))
            {
                // TODO: Check if region is in a whitelist?
                return BadRequest();
            }

            // Attempt to store user device information
            await this._registrationRepo.RegisterAsync(deviceId, region, ct);

            // TODO: Store device information
            return Ok();
        }
    }
}