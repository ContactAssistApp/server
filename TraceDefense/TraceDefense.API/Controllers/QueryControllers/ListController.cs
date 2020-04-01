using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.DAL.Services;
using TraceDefense.Entities.Protos;

namespace TraceDefense.API.Controllers.QueryControllers
{
    /// <summary>
    /// Handles requests to list <see cref="ProximityQuery"/> identifiers which are new to a client
    /// </summary>
    [Route("api/Query/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        /// <summary>
        /// <see cref="ProximityQuery"/> service layer
        /// </summary>
        private IProximityQueryService _queryService;

        /// <summary>
        /// Creates a new <see cref="ListController"/> instance
        /// </summary>
        /// <param name="queryService"><see cref="ProximityQuery"/> service layer</param>
        public ListController(IProximityQueryService queryService)
        {
            // Assign local values
            this._queryService = queryService;
        }

        /// <summary>
        /// Get <see cref="QueryInfo"/> for a region, starting at a provided timestamp
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Query/List?regionId=39%2c-74&amp;lastTimestamp=0
        ///     
        /// </remarks>
        /// <param name="regionId">Target region identifier</param>
        /// <param name="lastTimestamp">Latest <see cref="ProximityQuery"/> timestamp on client device, in ms from UNIX epoch</param>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <response code="404">No results found for request parameters</response>
        /// <returns>Collection of <see cref="QueryInfo"/> objects matching request parameters</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(QueryListResponse), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<QueryListResponse>> GetAsync(string regionId, long lastTimestamp)
        {
            CancellationToken ct = new CancellationToken();

            // Validate inputs
            if(String.IsNullOrEmpty(regionId))
            {
                return BadRequest();
            }
            if(lastTimestamp < 0)
            {
                return BadRequest();
            }

            // Pull queries matching parameters
            IEnumerable<QueryInfo> results = await this._queryService
                .GetLatestInfoAsync(regionId, lastTimestamp, ct);

            if (results != null)
            {
                // Convert to response proto
                QueryListResponse response = new QueryListResponse();
                response.Queryinfo.AddRange(results);

                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }
    }
}