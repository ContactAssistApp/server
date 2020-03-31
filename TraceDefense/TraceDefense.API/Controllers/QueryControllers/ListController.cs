using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.API.Models.Protos;
using TraceDefense.DAL.Services;
using TraceDefense.Entities.Interactions;

namespace TraceDefense.API.Controllers.QueryControllers
{
    /// <summary>
    /// Handles requests to list <see cref="Query"/> identifiers which are new to a client
    /// </summary>
    [Route("api/Query/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        /// <summary>
        /// Object mapping provider
        /// </summary>
        private IMapper _objectMap;
        /// <summary>
        /// <see cref="Query"/> service layer
        /// </summary>
        private IQueryService _queryService;

        /// <summary>
        /// Creates a new <see cref="ListController"/> instance
        /// </summary>
        /// <param name="objectMap">Object mapping provider</param>
        /// <param name="queryService"><see cref="Query"/> service layer</param>
        public ListController(IMapper objectMap, IQueryService queryService)
        {
            // Assign local values
            this._objectMap = objectMap;
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
        /// <param name="lastTimestamp">Latest <see cref="Query"/> timestamp on client device, in ms from UNIX epoch</param>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <response code="404">No results found for request parameters</response>
        /// <returns>Collection of <see cref="QueryInfo"/> objects matching request parameters</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(QueryListResponse), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet]
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
            IList<Entities.Interactions.QueryInfo> results = await this._queryService
                .GetLatestInfoAsync(regionId, lastTimestamp, ct);

            if (results != null)
            {
                // Cast to Proto
                QueryListResponse response = new QueryListResponse();
                response.Queryinfo.AddRange(
                    results.Select(r => this._objectMap.Map<Models.Protos.QueryInfo>(r))
                );

                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }
    }
}