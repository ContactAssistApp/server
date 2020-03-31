using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.API.Models.Protos;
using TraceDefense.DAL.Services;
using TraceDefense.Entities.Interactions;

namespace TraceDefense.API.Controllers.QueryControllers
{
    /// <summary>
    /// Handles <see cref="Query"/> size requests
    /// </summary>
    [Route("api/Query/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        /// <summary>
        /// <see cref="Query"/> service layer
        /// </summary>
        private IQueryService _queryService;

        /// <summary>
        /// Creates a new <see cref="SizeController"/> instance
        /// </summary>
        /// <param name="queryService"><see cref="Query"/> service layer</param>
        public SizeController(IQueryService queryService)
        {
            // Assign local values
            this._queryService = queryService;
        }

        /// <summary>
        /// Gets the total data size for a provided Region
        /// </summary>
        /// <param name="regionId">Region identifier</param>
        /// <param name="lastTimestamp">Timestamp of client's most recent <see cref="Query"/>, in ms since UNIX epoch</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Query/Size?regionId=39%2c-74&amp;lastTimestamp=0
        ///     
        /// </remarks>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <response code="404">No results found for request parameters</response>
        /// <returns>Query result size, in bytes</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(QuerySizeResponse), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<QuerySizeResponse>> GetAsync(string regionId, long lastTimestamp)
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

            // Get results
            long result = await this._queryService.GetLatestRegionSizeAsync(regionId, lastTimestamp, ct);

            if(result > 0)
            {
                return Ok(new QuerySizeResponse
                {
                    SizeOfQueryResponse = result
                });
            }
            else
            {
                return NotFound();
            }
        }
    }
}