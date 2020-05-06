﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using CovidSafe.API.v20200505.Protos;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Reports;
using CovidSafe.Entities.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidSafe.API.v20200505.Controllers.MessageControllers
{
    /// <summary>
    /// Handles requests for submitting <see cref="AreaMatch"/> messages
    /// </summary>
    [ApiController]
    [ApiVersion("2020-05-05")]
    [Route("api/Messages/[controller]")]
    public class AreaReportController : ControllerBase
    {
        /// <summary>
        /// AutoMapper instance for object resolution
        /// </summary>
        private readonly IMapper _map;
        /// <summary>
        /// <see cref="InfectionReport"/> service layer
        /// </summary>
        private readonly IInfectionReportService _reportService;

        /// <summary>
        /// Creates a new <see cref="AreaReportController"/> instance
        /// </summary>
        /// <param name="map">AutoMapper instance</param>
        /// <param name="reportService"><see cref="InfectionReport"/> service layer</param>
        public AreaReportController(IMapper map, IInfectionReportService reportService)
        {
            // Assign local values
            this._map = map;
            this._reportService = reportService;
        }

        /// <summary>
        /// Publish a <see cref="AreaMatch"/> for distribution among devices
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/Messages/AreaReport&amp;api-version=2020-05-05
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
                // Parse AreaMatch to AreaReport type
                AreaReport report = this._map.Map<AreaReport>(request);

                // Publish area
                await this._reportService.PublishAreaAsync(report, cancellationToken);
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