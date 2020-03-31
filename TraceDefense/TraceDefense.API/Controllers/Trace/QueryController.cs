using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraceDefense.API.Models.Trace;
using TraceDefense.DAL.Providers;
using TraceDefense.DAL.Services;
using TraceDefense.Entities.Schemas;

namespace TraceDefense.API.Controllers.Trace
{
    /// <summary>
    /// Handles trace query submissions
    /// </summary>
    [Route("api/Trace/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        /// <summary>
        /// <see cref="Query"/> service layer
        /// </summary>
        private IQueryService _queryService;


        /// <summary>
        /// Creates a new <see cref="QueryController"/> instance
        /// </summary>
        /// <param name="queryService"><see cref="Query"/> service layer</param>
        public QueryController(IQueryService queryService)
        {
            // Assign local values
            this._queryService = queryService;
        }

        /// <summary>
        /// Get queries for given query ids
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /Trace/Query?regionId=39%2c-74&amp;lastTimestamp=0
        /// </remarks>
        /// <response code="200">Successful request with results</response>
        /// <response code="400">Malformed or invalid request provided</response>
        /// <response code="404">No results found for request parameters</response>
        /// <returns>Collection of <see cref="Query"/> objects matching request parameters</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(QueryGetResponse), StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<QueryGetResponse>> GetAsync(string regionId, long lastTimestamp)
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
            IList<Query> result = await this._queryService.GetByRegionAsync(regionId, lastTimestamp, ct);

            if(result.Count > 0)
            {
                return Ok(new QueryGetResponse
                {
                    Queries = result,
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                });
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Publish a query for distribution among devices relevant to Area
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /PublishQueryRequest
        ///     {
        ///        "Query": { "tbd": "if you visited Trader Joe's on 3rd Ave 03/26/2020 between 18.00 and 19.00, call me" },
        ///        "Area": { 
        ///             "first": { "lat": 39.5, "lng": -74.5 },
        ///             "second": { "lat": 41.5, "lng": -72.5 }
        ///        }
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Query matched Trace results</response>
        /// <response code="400">Malformed or invalid query provided</response>
        /// <response code="404">No query results</response>
        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult> PutAsync(QueryPutRequest request)
        {
            CancellationToken ct = new CancellationToken();
            // TODO: Validate inputs

            var regions = RegionProvider.GetRegions(request.Area);
            await this._queryService.PublishAsync(regions, request.Query, ct);
            return Ok();
        }
    }
}